"""实机验证老编码、奇数尺寸、RGB/调色板/灰度及打包YUV的RTX软解上传。"""
import argparse
import json
import os
import socket
import subprocess
import time
import urllib.request
from pathlib import Path


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--runtime", required=True, type=Path)
    parser.add_argument("--work", required=True, type=Path)
    parser.add_argument("--rv40-sample", type=Path, help="可选RV40样本，测试只无损封装前四个视频包")
    args = parser.parse_args()
    runtime, work = args.runtime.resolve(), args.work.resolve()
    work.mkdir(parents=True, exist_ok=True)
    ffmpeg = runtime / "bin/ffmpeg/ffmpeg.exe"
    ffprobe = runtime / "bin/ffmpeg/ffprobe.exe"
    sidecar = runtime / "bin/rtx-video/runtime/vsr_backend.exe"
    with socket.socket() as listener:
        listener.bind(("127.0.0.1", 0))
        port = listener.getsockname()[1]
    session = "decode-fallback-regression"

    def request(path, payload=None):
        data = None if payload is None else json.dumps(payload).encode("utf-8")
        query = urllib.request.Request(f"http://127.0.0.1:{port}{path}", data=data,
            headers={"X-App-Session-Id": session, "Content-Type": "application/json"})
        with urllib.request.urlopen(query, timeout=10) as response:
            return json.load(response)

    cases = (
        ("mpeg4-odd", "mpeg4", "yuv420p", "343x259", [], "software + D3D11 upload"),
        ("mpeg4-XVID-unusual", "mpeg4", "yuv420p", "342x258", ["-vtag", "XVID"], "software + D3D11 upload"),
        ("msmpeg4v3", "msmpeg4v3", "yuv420p", "352x288", [], "software + D3D11 upload"),
        ("wmv2", "wmv2", "yuv420p", "352x288", [], "software + D3D11 upload"),
        ("h263", "h263", "yuv420p", "352x288", [], "software + D3D11 upload"),
        ("msvideo1", "msvideo1", "rgb555le", "352x288", [], "software + D3D11 upload"),
        ("rawvideo-pal8", "rawvideo", "pal8", "342x258", [], "software + D3D11 upload"),
        ("rv40-prefix", "copy", "yuv420p", "576x320", [], "software + D3D11 upload"),
        ("rawvideo-yuyv422", "rawvideo", "yuyv422", "342x258", [], "software + D3D11 upload"),
        ("rawvideo-bgr24", "rawvideo", "bgr24", "342x258", [], "software + D3D11 upload"),
        ("ffv1-gray", "ffv1", "gray", "342x258", [], "software + D3D11 upload"),
    )
    if args.rv40_sample is None:
        cases = tuple(case for case in cases if case[1] != "copy")
    results = []
    diagnostics_path = work / "VSR/logs/vsr_backend.log"
    with (work / "sidecar.log").open("w", encoding="utf-8") as log:
        child = subprocess.Popen([str(sidecar), "--port", str(port), "--app-session-id", session],
            cwd=sidecar.parent, stdout=log, stderr=log,
            env=dict(os.environ, LOCALAPPDATA=str(work)))
        try:
            for _ in range(100):
                try:
                    request("/api/health")
                    break
                except OSError:
                    time.sleep(0.1)
            capabilities = request("/api/capabilities")
            assert capabilities["vsrAvailable"] and capabilities["nvencH264Available"], capabilities
            (work / "capabilities.json").write_text(json.dumps(capabilities, ensure_ascii=False, indent=2), encoding="utf-8")
            for name, codec, pixel_format, size, extras, route in cases:
                source = work / (name + (".mkv" if codec == "copy" else ".avi"))
                output = work / (name + "-output.mkv")
                output.unlink(missing_ok=True)
                if codec == "copy":
                    subprocess.run([str(ffmpeg), "-y", "-v", "error", "-i", str(args.rv40_sample.resolve()),
                        "-map", "0:v:0", "-frames:v", "4", "-c:v", "copy", str(source)], check=True, capture_output=True)
                else:
                    subprocess.run([str(ffmpeg), "-y", "-v", "error", "-f", "lavfi", "-i",
                        f"testsrc=size={size}:rate=4", "-f", "lavfi", "-i", "sine=duration=1",
                        "-t", "1", "-c:v", codec, "-pix_fmt", pixel_format, "-c:a", "aac",
                        *extras, str(source)], check=True, capture_output=True)
                log.flush()
                start = diagnostics_path.stat().st_size
                job = request("/api/jobs", {"inputPath": str(source), "outputPath": str(output),
                    "processing": {"vsr": {"enabled": True, "quality": 3, "scale": 2},
                        "hdr": {"enabled": False}},
                    "output": {"container": "matroska", "videoCodec": "h264",
                        "audioMode": "copy", "subtitleMode": "none", "pixelFormat": "nv12"}})
                deadline = time.monotonic() + 120
                while time.monotonic() < deadline:
                    status = request("/api/jobs/" + job["id"])
                    if status["state"] in ("succeeded", "failed", "canceled"):
                        break
                    time.sleep(0.1)
                assert status["state"] == "succeeded", status
                metadata = json.loads(subprocess.check_output([str(ffprobe), "-v", "error",
                    "-count_frames", "-show_streams", "-of", "json", str(output)], encoding="utf-8"))
                video = next(stream for stream in metadata["streams"] if stream["codec_type"] == "video")
                source_video = json.loads(subprocess.check_output([str(ffprobe), "-v", "error",
                    "-count_frames", "-select_streams", "v:0", "-show_streams", "-of", "json", str(source)], encoding="utf-8"))["streams"][0]
                width, height = source_video["width"], source_video["height"]
                frames = int(source_video["nb_read_frames"])
                if codec != "copy":
                    assert (width, height, frames) == (*map(int, size.split("x")), 4), source_video
                assert (video["width"], video["height"], int(video["nb_read_frames"])) == (width * 2, height * 2, frames), metadata
                audio_frames = "none"
                if codec != "copy":
                    audio = next(stream for stream in metadata["streams"] if stream["codec_type"] == "audio")
                    source_audio = json.loads(subprocess.check_output([str(ffprobe), "-v", "error",
                        "-select_streams", "a:0", "-count_frames", "-show_streams", "-of", "json",
                        str(source)], encoding="utf-8"))["streams"][0]
                    assert audio["nb_read_frames"] == source_audio["nb_read_frames"], (audio, source_audio)
                    audio_frames = audio["nb_read_frames"]
                with diagnostics_path.open("rb") as stream:
                    stream.seek(start)
                    diagnostics = stream.read().decode("utf-8", errors="replace")
                assert "Video decode route: " + route in diagnostics, diagnostics
                results.append({"case": name, "route": route, "frames": frames,
                    "audioFrames": audio_frames, "width": video["width"], "height": video["height"]})
                (work / "results.json").write_text(json.dumps(results, ensure_ascii=False, indent=2), encoding="utf-8")
                print("PASS", name, route, flush=True)
        finally:
            child.terminate()
            child.wait(timeout=10)


if __name__ == "__main__":
    main()
