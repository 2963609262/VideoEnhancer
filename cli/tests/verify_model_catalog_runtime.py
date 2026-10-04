"""在隔离运行目录验证模型归组、导入兼容性与真实输出尺寸。"""
import argparse
import json
import os
import shutil
import subprocess
from pathlib import Path


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--runtime", required=True)
    parser.add_argument("--extra-only", action="store_true")
    parser.add_argument("--final-only", action="store_true")
    parser.add_argument("--scale-fix-only", action="store_true")
    args = parser.parse_args()
    runtime = Path(args.runtime).resolve()
    work = runtime.parent / "verification"
    work.mkdir(exist_ok=True)
    exe = runtime / "videoenhancer.exe"
    ffmpeg = runtime / "bin/ffmpeg/ffmpeg.exe"
    ffprobe = runtime / "bin/ffmpeg/ffprobe.exe"
    results = []
    environment = dict(os.environ, PYTHONUTF8="1")

    def run(name, arguments, expected=0, timeout=300):
        child = subprocess.run([str(exe), *map(str, arguments), "--ffmpeg-path", str(ffmpeg), "--ffprobe-path", str(ffprobe)],
                               cwd=work, env=environment, capture_output=True, encoding="utf-8", errors="replace", timeout=timeout)
        (work / (name + ".log")).write_text(child.stdout + "\n" + child.stderr, encoding="utf-8")
        assert child.returncode == expected, f"{name}: {child.returncode}, {(child.stdout + child.stderr)[-1500:]}"
        return child.stdout

    def record(name):
        results.append(name)
        result_name = "results-scale-fix.json" if args.scale_fix_only else "results-final.json" if args.final_only else "results-extra.json" if args.extra_only else "results.json"
        (work / result_name).write_text(json.dumps(results, ensure_ascii=False, indent=2), encoding="utf-8")
        print("PASS", name, flush=True)

    def probe(path):
        child = subprocess.run([str(ffprobe), "-v", "error", "-select_streams", "v:0", "-count_frames",
                                "-show_entries", "stream=width,height,nb_read_frames", "-of", "json", str(path)],
                               capture_output=True, encoding="utf-8", check=True)
        return json.loads(child.stdout)["streams"][0]

    def json_line(text):
        return json.loads(next(line for line in reversed(text.splitlines()) if line.startswith(("[", "{"))))

    # 短片与图片尺寸相同，避免重复推理验证无关路径。
    image = work / "source.png"
    video = work / "source.mkv"
    for output, extras in [(image, ["-frames:v", "1"]), (video, ["-frames:v", "4", "-c:v", "ffv1"])]:
        subprocess.run([str(ffmpeg), "-y", "-v", "error", "-f", "lavfi", "-i", "testsrc2=size=96x64:rate=4",
                        *extras, str(output)], check=True)
    renamed = work / "renamed-weight.pth"
    shutil.copyfile(runtime / "models/PTH/realesr-animevideov3.pth", renamed)
    imported = json_line(run("import-renamed", ["--json", "--import-model", renamed]))[0]
    assert imported["success"], imported
    catalog = json_line(run("catalog-cuda", ["--json", "--list-model-catalog", "-backend", "cuda"]))
    builtin = next(item for item in catalog if item["id"] == "PTH/realesr-animevideov3")
    user = next(item for item in catalog if item["id"] == imported["id"])
    assert builtin["architectureGroup"] == user["architectureGroup"] == "Compact"
    assert builtin["scale"] == user["scale"] == 4
    record("same-weight-renamed-import")
    catalog_file = runtime / "models/User/model-catalog.json"
    user_manifest = json.loads(catalog_file.read_text(encoding="utf-8"))
    next(item for item in user_manifest["models"] if item["id"] == imported["id"])["scale"] = 2
    catalog_file.write_text(json.dumps(user_manifest, ensure_ascii=False), encoding="utf-8")
    saved_bytes = catalog_file.read_bytes()
    catalog = json_line(run("catalog-legacy-manual", ["--json", "--list-model-catalog", "-backend", "cuda"]))
    assert next(item for item in catalog if item["id"] == imported["id"])["scale"] == 4
    assert catalog_file.read_bytes() == saved_bytes
    record("legacy-manual-record-preserved-native-detected")
    base_video = ["-i", video, "-modelpath", "PTH/realesr-animevideov3", "-backend", "cuda"]
    settings = f'-c:v ffv1 "{work / "invalid.mkv"}" -y'
    error = run("reject-inference-override", [*base_video, "-scale", "2", "-ffmpeg-settings", settings], expected=2)
    record("fixed-weight-rejects-scale-override")
    run("reject-output-range", [*base_video, "-output-scale", "17", "-ffmpeg-settings", settings], expected=2)
    record("output-scale-range")

    def video_case(name, backend, model, target, native, extra=()):
        output = work / (name + ".mkv")
        arguments = ["-i", video, "-modelpath", model, "-backend", backend,
                     "-ffmpeg-settings", f'-c:v ffv1 "{output}" -y', *extra]
        if target:
            arguments += ["-output-scale", str(target)]
        run(name, arguments, timeout=600)
        metadata = probe(output)
        effective = target or native
        assert (metadata["width"], metadata["height"]) == (96 * effective, 64 * effective), metadata
        # RIFE 在相邻源帧间补一帧：四个源帧和三个间隙，共七帧。
        expected_frames = 7 if "-interp-factor" in extra else 4
        assert int(metadata["nb_read_frames"]) == expected_frames, metadata
        record(name)

    if args.scale_fix_only:
        for backend, model, target, native in (
            ("cuda", "PTH/realesr-animevideov3", 2, 4),
            ("tensorrt", "PTH/realesr-animevideov3", 2, 4),
            ("ncnn", "Param-Bin/RealESRGAN-AnimeVideoV3-2x", 1, 2),
            ("onnx", "ONNX/AniSD-AC-G6i2a-Compact-72500-fp32-2x", 3, 2),
            ("basicvsrpp", "BasicVSR++/basicvsr_plusplus_c64n7_8x1_600k_reds4_20210217-db622b2f", 2, 4),
            ("flashvsr", "FlashVSR", 2, 4),
            ("flashvsr", "FlashVSR", 3, 4),
        ):
            video_case(f"fix-{backend}-{target}", backend, model, target, native)
        interp = ["-interp-model", "Frame-Interpolation/RIFE/rife4.25.pkl", "-interp-backend", "cuda", "-interp-factor", "2"]
        for order in ("upscale-first", "interp-first"):
            video_case("fix-combined-" + order, "cuda", "PTH/realesr-animevideov3", 2, 4,
                [*interp, "-process-order", order])
        video_case("fix-cross-backend", "ncnn", "Param-Bin/RealESRGAN-AnimeVideoV3-2x", 3, 2, interp)
        for backend in ("cuda", "tensorrt"):
            output = work / ("fix-image-" + backend)
            output.mkdir(exist_ok=True)
            run(output.name, ["--image-input", image, "--image-output", output,
                "-modelpath", "PTH/realesr-animevideov3", "-backend", backend, "-output-scale", "2"])
            metadata = probe(next(output.glob("*.png")))
            assert (metadata["width"], metadata["height"]) == (192, 128), metadata
            record(output.name)
        engines = list((runtime / "models/TensorRT-Cache").rglob("*__scale-2*.engine"))
        assert engines, "缺少实际输出倍率为 2x 的 TRT Engine"
        record("fix-trt-cache-output-scale-2")
        print(f"Verified {len(results)} scale fix scenarios", flush=True)
        return

    if args.final_only:
        restoration = work / "renamed-restoration.pth"
        shutil.copyfile(runtime / "models/PTH/AniScale2-Refiner-10K-1x.pth", restoration)
        imported_restoration = json_line(run("import-restoration", ["--json", "--import-model", restoration]))[0]
        assert imported_restoration["success"], imported_restoration
        for backend in ("cuda", "tensorrt"):
            catalog = json_line(run("catalog-restoration-" + backend, ["--json", "--list-model-catalog", "-backend", backend]))
            entry = next(item for item in catalog if item["id"] == imported_restoration["id"])
            assert entry["architectureGroup"] == "Compact" and entry["scale"] == 1, entry
        record("imported-1x-restoration-visible-in-supported-backends")
        misleading_onnx = work / "misleading-4x.onnx"
        shutil.copyfile(runtime / "models/ONNX/AniSD-AC-G6i2a-Compact-72500-fp32-2x.onnx", misleading_onnx)
        inspected = json_line(run("inspect-misleading-onnx", ["--inspect-upscale-model", misleading_onnx]))
        assert inspected["scale"] == 2 and inspected["architecture"] == "Compact", inspected
        record("onnx-filename-does-not-override-native-scale")
        renamed_swinir = work / "renamed-transformer.onnx"
        shutil.copyfile(runtime / "models/ONNX/AniSD-AC-G6i2b-SwinIR-117500-240x320-fp32-2x.onnx", renamed_swinir)
        inspected = json_line(run("inspect-renamed-swinir", ["--inspect-upscale-model", renamed_swinir]))
        assert inspected["architecture"] == "SwinIR" and inspected["scale"] == 2, inspected
        record("onnx-swinir-graph-family")
        output = work / "video-existing-filter.mkv"
        run("video-existing-filter", [*base_video, "-output-scale", "3", "-ffmpeg-settings",
                                      f'-vf crop=64:48:0:0 -s 640x480 -c:v ffv1 "{output}" -y'])
        metadata = probe(output)
        assert (metadata["width"], metadata["height"]) == (288, 192), metadata
        record("existing-filter-then-final-target-scale")
        print(f"Verified {len(results)} final scenarios", flush=True)
        return

    if args.extra_only:
        renamed_ncnn = work / "renamed-ncnn"
        shutil.copytree(runtime / "models/Param-Bin/RealESRGAN-AnimeVideoV3-2x", renamed_ncnn, dirs_exist_ok=True)
        imported_ncnn = json_line(run("import-ncnn-renamed", ["--json", "--import-model", renamed_ncnn]))[0]
        assert imported_ncnn["success"], imported_ncnn
        ncnn_catalog = json_line(run("catalog-ncnn", ["--json", "--list-model-catalog", "-backend", "ncnn"]))
        entry = next(item for item in ncnn_catalog if item["id"] == imported_ncnn["id"])
        assert entry["architectureGroup"] == "Compact" and entry["scale"] == 2, entry
        record("ncnn-renamed-graph-import")
        video_case("ncnn-imported-2x-to-3x", "ncnn", imported_ncnn["id"], 3, 2)
        renamed_onnx = work / "renamed-graph.onnx"
        shutil.copyfile(runtime / "models/ONNX/AniSD-AC-G6i2a-Compact-72500-fp32-2x.onnx", renamed_onnx)
        inspected = json_line(run("inspect-renamed-onnx", ["--inspect-upscale-model", renamed_onnx]))
        assert inspected["architecture"] == "Compact" and inspected["scale"] == 2, inspected
        record("onnx-renamed-structure-and-scale")
        video_case("restoration-tensorrt-1x-to-2x", "tensorrt", "PTH/AniScale2-Refiner-10K-1x", 2, 1)
        # 两种同后端顺序及跨后端均在结果进入下游前落实目标尺寸。
        interp = ["-interp-model", "Frame-Interpolation/RIFE/rife4.25.pkl", "-interp-backend", "cuda", "-interp-factor", "2"]
        for order in ("upscale-first", "interp-first"):
            video_case("combined-cuda-" + order, "cuda", "PTH/realesr-animevideov3", 3, 4,
                       [*interp, "-process-order", order])
        video_case("combined-ncnn-cuda", "ncnn", "Param-Bin/RealESRGAN-AnimeVideoV3-2x", 3, 2, interp)
        video_case("flashvsr-direct-2x", "flashvsr", "FlashVSR", 2, 4)
        text = (work / "flashvsr-direct-2x.log").read_text(encoding="utf-8")
        progress = next(line for line in text.splitlines() if line.startswith("FLASHVSR_PROCESS|"))
        assert progress.split("|")[3] == "2", progress
        print(f"Verified {len(results)} extra scenarios", flush=True)
        return

    for backend in ("cuda", "tensorrt"):
        for target in (0, 2, 3, 4):
            name = f"video-{backend}-{target or 'native'}"
            video_case(name, backend, "PTH/realesr-animevideov3", target, 4)
            directory = work / f"image-{backend}-{target or 'native'}"
            directory.mkdir(exist_ok=True)
            arguments = ["--image-input", image, "--image-output", directory,
                         "-modelpath", "PTH/realesr-animevideov3", "-backend", backend]
            if target:
                arguments += ["-output-scale", str(target)]
            run(directory.name, arguments, timeout=600)
            metadata = probe(next(directory.glob("*.png")))
            effective = target or 4
            assert (metadata["width"], metadata["height"]) == (96 * effective, 64 * effective), metadata
            record(directory.name)
    # 缓存按实际输出倍率隔离；低目标倍率应生成图内缩放的 Engine。
    engines = list((runtime / "models/TensorRT-Cache").rglob("*.engine"))
    assert engines and all(any(f"__scale-{scale}" in item.name for scale in (2, 3, 4)) for item in engines), [item.name for item in engines]
    record("tensorrt-cache-output-scale-isolated")
    video_case("ncnn-compiled-2x-to-3x", "ncnn", "Param-Bin/RealESRGAN-AnimeVideoV3-2x", 3, 2)
    video_case("onnx-fixed-2x-to-3x", "onnx", "ONNX/AniSD-AC-G6i2a-Compact-72500-fp32-2x", 3, 2)
    video_case("restoration-native-1x-to-2x", "cuda", "PTH/AniScale2-Refiner-10K-1x", 2, 1)
    # 专用时序后端也在写入编码器前调整目标尺寸。
    video_case("basicvsrpp-4x-to-2x", "basicvsrpp", "BasicVSR++/basicvsr_plusplus_c64n7_8x1_600k_reds4_20210217-db622b2f", 2, 4)
    print(f"Verified {len(results)} scenarios", flush=True)


if __name__ == "__main__":
    main()
