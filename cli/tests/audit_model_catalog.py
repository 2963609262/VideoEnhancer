"""逐项读取本机模型结构，生成能力清单审计证据；不修改模型。"""
import argparse
import importlib.util
import json
import hashlib
import sys
from pathlib import Path


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--runtime", required=True)
    parser.add_argument("--output", required=True)
    args = parser.parse_args()
    runtime = Path(args.runtime)
    sys.path.insert(0, str(runtime / "python/backend"))
    spec = importlib.util.spec_from_file_location("inspector", Path(__file__).parents[1] / "embedded-tools/inspect_upscale_models.py")
    inspector = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(inspector)
    manifest = json.loads((Path(__file__).parents[1] / "model-capabilities.json").read_text(encoding="utf-8"))
    evidence = []
    for entry in manifest["models"]:
        path = runtime / "models" / entry["model"]
        row = {"model": entry["model"], "declared": entry}
        if entry["model"].startswith(("PTH/", "ONNX/")):
            suffixes = (".pth", ".pt", ".ckpt", ".safetensors") if entry["model"].startswith("PTH/") else (".onnx",)
            actual = next((Path(str(path) + ext) for ext in suffixes if Path(str(path) + ext).is_file()), None)
            row["inspection"] = inspector.inspect_model(str(actual)) if actual else {"error": "本机缺少权重，待复核"}
            if actual:
                with actual.open("rb") as stream:
                    row["sha256"] = hashlib.file_digest(stream, "sha256").hexdigest()
            if actual and actual.suffix == ".onnx":
                import onnx
                from collections import Counter
                graph = onnx.load(str(actual)).graph
                row["graph_ops"] = dict(Counter(node.op_type for node in graph.node))
                row["graph_names"] = [node.name for node in graph.node if node.op_type in {"DepthToSpace", "Resize", "Conv", "ConvTranspose"}]
        elif entry["model"].startswith("Param-Bin/"):
            params = list(path.glob("*.param"))
            row["param_layers"] = [line for p in params for line in p.read_text(encoding="utf-8").splitlines() if line.startswith(("PixelShuffle", "Interp", "Convolution", "Deconvolution", "Padding", "Crop"))]
            row["files"] = [p.name for p in path.glob("*")]
            row["param_sha256"] = [hashlib.sha256(p.read_bytes()).hexdigest() for p in params]
        else:
            row["files"] = [p.name for p in path.glob("*")]
            if entry["model"].startswith("BasicVSR++/"):
                from src.basicvsrpp.model import load_basicvsrpp_model
                model, checkpoint = load_basicvsrpp_model(str(path.with_suffix(".pth")))
                row["inspection"] = {"architecture": type(model).__name__,
                                     "scale": 4 if model.is_low_res_input else 1,
                                     "error": "", "backends": ["basicvsrpp"]}
                with open(checkpoint, "rb") as stream:
                    row["sha256"] = hashlib.file_digest(stream, "sha256").hexdigest()
            if (path / "config.py").is_file():
                row["config"] = (path / "config.py").read_text(encoding="utf-8")
        evidence.append(row)
        print(entry["model"], row.get("inspection", {}).get("architecture", "graph/config"), row.get("inspection", {}).get("error", ""), flush=True)
    Path(args.output).write_text(json.dumps(evidence, ensure_ascii=False, indent=2), encoding="utf-8")


if __name__ == "__main__":
    main()
