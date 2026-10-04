"""实机验证 RTX 只处理可见画面，不放大编码填充区，保留裁剪偏移。"""
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
        ("h264-crop-right-bottom", "libx264", "yuv420p", "342x258", ["-vf", "pad=384:288:0:0:black", "-bsf:v", "h264_metadata=crop_right=42:crop_bottom=30"], "D3D11VA"),
        ("h264-crop-left-top", "libx264", "yuv420p", "342x258", ["-vf", "pad=384:288:42:30:black", "-bsf:v", "h264_metadata=crop_left=42:crop_top=30"], "D3D11VA"),
        ("h264-unaligned-control", "libx264", "yuv420p", "426x240", [], "D3D11VA"),
        ("h264-high10-left-top", "libx264", "yuv420p10le", "342x258", ["-vf", "pad=384:288:42:30:black", "-bsf:v", "h264_metadata=crop_left=42:crop_top=30"], "software + D3D11 upload"),
        ("h264-high10-crop", "libx264", "yuv420p10le", "342x258", ["-vf", "pad=384:288:0:0:black", "-bsf:v", "h264_metadata=crop_right=42:crop_bottom=30"], "software + D3D11 upload"),
    )
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
                source = work / (name + (".avi" if codec == "mpeg4" else ".mkv"))
                output = work / (name + "-output.mkv")
                output.unlink(missing_ok=True)
                subprocess.run([str(ffmpeg), "-y", "-v", "error", "-f", "lavfi", "-i",
                    f"color=white:size={size}:rate=1", "-f", "lavfi", "-i", "sine=duration=1",
                    "-t", "1", "-c:v", codec, "-pix_fmt", pixel_format, "-c:a", "aac",
                    *extras, str(source)], check=True, capture_output=True)
                log.flush()
                start = diagnostics_path.stat().st_size
                job = request("/api/jobs", {"inputPath": str(source), "outputPath": str(output),
                    "processing": {"vsr": {"enabled": True, "quality": 3, "scale": 4},
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
                width, height = map(int, size.split("x"))
                assert (video["width"], video["height"], int(video["nb_read_frames"])) == (width * 4, height * 4, 1), metadata
                import numpy as np
                raw = subprocess.check_output([str(ffmpeg), "-v", "error", "-i", str(output), "-frames:v", "1", "-f", "rawvideo", "-pix_fmt", "gray", "-"])
                gray = np.frombuffer(raw, dtype=np.uint8).reshape(height * 4, width * 4)
                yy, xx = np.where(gray > 64)
                bounds = [int(xx.min()), int(yy.min()), int(xx.max())+1, int(yy.max())+1]
                border = {"size": size, "scale": 4, "bounds": bounds, "expected": [0, 0, width * 4, height * 4], "blackPixels": int((gray < 16).sum())}
                assert bounds == border["expected"] and border["blackPixels"] == 0, border
                print("BORDER_RESULT " + json.dumps(border), flush=True)
                (work / (name + "-border.json")).write_text(json.dumps(border), encoding="utf-8")
                audio = next(stream for stream in metadata["streams"] if stream["codec_type"] == "audio")
                source_audio = json.loads(subprocess.check_output([str(ffprobe), "-v", "error",
                    "-select_streams", "a:0", "-count_frames", "-show_streams", "-of", "json",
                    str(source)], encoding="utf-8"))["streams"][0]
                assert audio["nb_read_frames"] == source_audio["nb_read_frames"], (audio, source_audio)
                with diagnostics_path.open("rb") as stream:
                    stream.seek(start)
                    diagnostics = stream.read().decode("utf-8", errors="replace")
                assert "Video decode route: " + route in diagnostics, diagnostics
                results.append({"case": name, "route": route, "frames": 1,
                    "audioFrames": audio["nb_read_frames"], "width": video["width"], "height": video["height"]})
                (work / "results.json").write_text(json.dumps(results, ensure_ascii=False, indent=2), encoding="utf-8")
                print("PASS", name, route, flush=True)
        finally:
            child.terminate()
            child.wait(timeout=10)


if __name__ == "__main__":
    main()
