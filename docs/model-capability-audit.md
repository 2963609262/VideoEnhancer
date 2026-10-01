# 模型架构与倍率审计（2026-10-01）

本轮覆盖内置清单全部 98 项：47 个 PTH、28 个 ONNX、21 个 NCNN 图与 2 个专用视频后端。逐项证据见 [审计数据](model-capability-audit.json)。权重和模型文件没有修改，模型 ID 与安装路径保持不变。

## 核对方法与边界

- PTH 使用当前后端的真实注册器加载权重，记录网络类、倍率与输入约束；AnimeSR 使用专用类严格加载。
- ONNX 读取图结构与固定输入输出尺寸；22 个动态图以 64×64 输入进行 CPU 输出形状探测，倍率不再来自文件名。架构按图中网络结构识别，未知图归入“其他模型”。
- NCNN 核对 param 图与图内缩放节点，登记内容 SHA256。导入相同图即使改目录名也得到同一架构与倍率；未收录图保留未知架构，不冒充已知家族。
- BasicVSR++ 严格加载检查键集合，并按 is_low_res_input 确定 4x 或 1x。FlashVSR 核对组件和当前实现的 2x/4x 参数，本轮实际验证直接 2x。
- 结构核对不等同于对每个模型完成 GPU 推理；实际运行验证采用必要代表模型，后端范围未整体扩大。

## 主要修正

- `realesr-animevideov3`、General v3、AniScale2 Refiner、AniSD DB i2 等实为 Compact；训练系列和文件名不再当作网络架构。
- RealESRGAN x2plus/x4plus/anime 6B 与 APISR RRDB 按 ESRGAN / RRDB 分组。RealHatGAN ONNX 按 HAT 结构归类。
- ESRGAN-Lite、DAT2、RealPLKSR L/S 等变体归入对应家族，具体网络名称保留；SPANPlus 与 SPAN 分列。
- RRDB 2x 输入倍数补为 4；DITN 为 8；CRAFT 为 16。原有已知更严格约束保留。
- 修复架构描述为字符串时预检失败、AnimeSR 专用识别、导入 NCNN 目录名与文件名不匹配、导入 1x 修复模型被工作台排除的问题。
- Compact 1x 的导入能力与内置能力统一为 CUDA/TensorRT；已实际验证该类 1x TensorRT 模型。

## 倍率接口与兼容

`scale` 继续表示模型/导出图的原生输出倍率；`architectureGroup` 表示菜单家族，`architecture` 保留具体架构。`inferenceScales` 只登记当前后端明确支持的多倍率，FlashVSR 为 `[2, 4]`。

新增 `-output-scale 1–16`，不传时保持原生。固定倍率模型先原生推理，再在最终编码前或图片保存前用 Lanczos 调整输出。已验证的后端多倍率可直接推理。旧 `-scale` 仍表示推理倍率，不能把固定 4x 权重声明成 2x。TensorRT 缓存只受实际推理倍率影响。

旧用户清单不自动改写、不移动文件。按文件大小和修改时间缓存实际检测，菜单和执行使用检测到的原生倍率；用户保存的修正仍可在管理页查看。重新检测先展示差异，再由用户选择载入修正窗口并保存。
显式目标倍率在现有视频滤镜之后应用，并替代编码预设的 -s 输出尺寸。旧配置没有 OutputScale 时默认原生；RTX VSR 与分段超分继续使用各自输出规格，通用倍率不覆盖它们。

## 逐项结果

| 模型 ID | 具体架构 | 菜单分组 | 原生倍率 | 输入倍数 | 证据 |
| --- | --- | --- | ---: | ---: | --- |
| `BasicVSR++/basicvsr_plusplus_c64n7_8x1_600k_reds4_20210217-db622b2f` | BasicVSR++ | BasicVSR++ | 4x | 1 | BasicVSR++严格加载 |
| `FlashVSR` | FlashVSR | FlashVSR | 4x | 1 | FlashVSR组件与后端实现；2x实际推理 |
| `ONNX/AnimeJaNai-HD-V3.1-Balanced-SPANF3-b8f64-unshuffle-fp16-2x` | SPANF3 | SPAN | 2x | 1 | ONNX图与输入输出 |
| `ONNX/AnimeJaNai-HD-V3.1-Performance-SPANF3-b5f48-unshuffle-fp16-2x` | SPANF3 | SPAN | 2x | 1 | ONNX图与输入输出 |
| `ONNX/AnimeJaNai-HD-V3.1Sharp1-Balanced-SPANF3-b8f64-unshuffle-fp16-2x` | SPANF3 | SPAN | 2x | 1 | ONNX图与输入输出 |
| `ONNX/AnimeJaNai-HD-V3.1Sharp1-Performance-SPANF3-b5f48-unshuffle-fp16-2x` | SPANF3 | SPAN | 2x | 1 | ONNX图与输入输出 |
| `ONNX/AnimeJaNai-SD-V1beta34-Compact-1x3xHxW-dyn-HW-strong-fp16-op21-dynamo-2x` | Compact | Compact | 2x | 1 | ONNX图与输入输出 |
| `ONNX/AniSD-AC-G6i2a-Compact-72500-fp32-2x` | Compact | Compact | 2x | 1 | ONNX图与输入输出 |
| `ONNX/AniSD-AC-G6i2b-SPAN-190K-fp32-2x` | SPAN | SPAN | 2x | 1 | ONNX图与输入输出 |
| `ONNX/AniSD-AC-G6i2b-SwinIR-117500-240x320-fp32-2x` | SwinIR | SwinIR | 2x | 1 | ONNX图与输入输出 |
| `ONNX/AniSD-AC-G6i2b-SwinIR-117500-320x448-fp32-2x` | SwinIR | SwinIR | 2x | 1 | ONNX图与输入输出 |
| `ONNX/AniSD-AC-G6i2b-SwinIR-117500-480x320-fp32-2x` | SwinIR | SwinIR | 2x | 1 | ONNX图与输入输出 |
| `ONNX/AniSD-AC-RealPLKSR-127500-fp32-FO-dynamic-2x` | RealPLKSR | RealPLKSR | 2x | 1 | ONNX图与输入输出 |
| `ONNX/AniSD-DB-i2-SPAN-85K-fp32-1x` | Compact | Compact | 1x | 1 | ONNX图与输入输出 |
| `ONNX/AniSD-DC-DAT2-97500-fp32FO-2x` | DAT | DAT | 2x | 1 | ONNX图与输入输出 |
| `ONNX/AniSD-DC-RealPLKSR-115K-fp32-FO-dynamic-2x` | RealPLKSR | RealPLKSR | 2x | 1 | ONNX图与输入输出 |
| `ONNX/AniSD-DC-SPAN-92500-fp32-2x` | SPAN | SPAN | 2x | 1 | ONNX图与输入输出 |
| `ONNX/AniSD-G6i1-SPAN-215K-fp32-2x` | SPAN | SPAN | 2x | 1 | ONNX图与输入输出 |
| `ONNX/AniSD-G6i1-SwinIR-165K-240x320-fp32-2x` | SwinIR | SwinIR | 2x | 1 | ONNX图与输入输出 |
| `ONNX/AniSD-G6i1-SwinIR-165K-320x448-fp32-2x` | SwinIR | SwinIR | 2x | 1 | ONNX图与输入输出 |
| `ONNX/AniSD-G6i1-SwinIR-165K-480x320-fp32-2x` | SwinIR | SwinIR | 2x | 1 | ONNX图与输入输出 |
| `ONNX/AniSD-G6i1b-Compact-102500-fp32-2x` | Compact | Compact | 2x | 1 | ONNX图与输入输出 |
| `ONNX/AniSD-PS-G6i2-Compact-50K-fp32-2x` | Compact | Compact | 2x | 1 | ONNX图与输入输出 |
| `ONNX/AniSD-RealPLKSR-140K-fp32-FO-dynamic-2x` | RealPLKSR | RealPLKSR | 2x | 1 | ONNX图与输入输出 |
| `ONNX/RealESRGAN-x4-jp-Illustration-fix1-d` | ESRGAN | ESRGAN / RRDB | 4x | 1 | ONNX图与输入输出 |
| `ONNX/RealESRGAN-x4-jp-Illustration-fix2` | ESRGAN | ESRGAN / RRDB | 4x | 1 | ONNX图与输入输出 |
| `ONNX/RealHatGAN-JP-Illustration-2x-fix1` | HAT | HAT | 2x | 16 | ONNX图与输入输出 |
| `ONNX/RealHatGAN-JP-Illustration-4x-fix1` | HAT | HAT | 4x | 16 | ONNX图与输入输出 |
| `ONNX/RealHatGAN-Universal-Illustration-2x-fix1` | HAT | HAT | 2x | 16 | ONNX图与输入输出 |
| `ONNX/RealHatGAN-x1-jp-Illustration-fix-only` | HAT | HAT | 1x | 16 | ONNX图与输入输出 |
| `Param-Bin/AnimeJaNai-V2-2x-Compact-36K` | Compact | Compact | 2x | 1 | NCNN param图 |
| `Param-Bin/AnimeJaNai-V3-2x-HD-Sharp1-Compact-430K` | Compact | Compact | 2x | 1 | NCNN param图 |
| `Param-Bin/AniSD-DC-SPAN-2x-92500` | SPAN | SPAN | 2x | 1 | NCNN param图 |
| `Param-Bin/CUGAN-Conservative-2x` | Real-CUGAN | Real-CUGAN | 2x | 1 | NCNN param图 |
| `Param-Bin/DenoiseH264-SuperUltraCompact-1x-float16` | Compact | Compact | 1x | 1 | NCNN param图 |
| `Param-Bin/DnCNN-ColorBlind-1x` | DnCNN | DnCNN | 1x | 1 | NCNN param图 |
| `Param-Bin/ModernSpanimation-V2-2x` | SPAN | SPAN | 2x | 1 | NCNN param图 |
| `Param-Bin/Nomos8k-span-otf-4x-medium` | SPAN | SPAN | 4x | 1 | NCNN param图 |
| `Param-Bin/Nomos8k-span-otf-4x-strong` | SPAN | SPAN | 4x | 1 | NCNN param图 |
| `Param-Bin/OpenProteus-Compact-i2-2x-70K` | Compact | Compact | 2x | 1 | NCNN param图 |
| `Param-Bin/RealESRGAN-AnimeVideoV3-2x` | Compact | Compact | 2x | 1 | NCNN param图 |
| `Param-Bin/RealESRGAN-AnimeVideoV3-3x` | Compact | Compact | 3x | 1 | NCNN param图 |
| `Param-Bin/RealESRGAN-AnimeVideoV3-4x` | Compact | Compact | 4x | 1 | NCNN param图 |
| `Param-Bin/RealESRGAN-General-x4v3` | Compact | Compact | 4x | 1 | NCNN param图 |
| `Param-Bin/RealESRGAN-x4plus-Anime-4x` | ESRGAN | ESRGAN / RRDB | 4x | 1 | NCNN param图 |
| `Param-Bin/Waifu2x-2x` | Waifu2x | Waifu2x | 2x | 1 | NCNN param图 |
| `Param-Bin/Waifu2x-Noise0-2x` | Waifu2x | Waifu2x | 2x | 1 | NCNN param图 |
| `Param-Bin/Waifu2x-Noise1-2x` | Waifu2x | Waifu2x | 2x | 1 | NCNN param图 |
| `Param-Bin/Waifu2x-Noise2-2x` | Waifu2x | Waifu2x | 2x | 1 | NCNN param图 |
| `Param-Bin/Waifu2x-Noise3-2x` | Waifu2x | Waifu2x | 2x | 1 | NCNN param图 |
| `Param-Bin/Waifu2x-Photo-2x` | Waifu2x | Waifu2x | 2x | 1 | NCNN param图 |
| `PTH/Ani4K-Compact-35000-2x` | RealESRGAN Compact | Compact | 2x | 1 | 权重严格加载与描述器 |
| `PTH/AnimeJaNai-V2-2x-Compact-36K` | RealESRGAN Compact | Compact | 2x | 1 | 权重严格加载与描述器 |
| `PTH/AnimeJaNai-V3-2x-HD-Sharp1-Compact-430K` | RealESRGAN Compact | Compact | 2x | 1 | 权重严格加载与描述器 |
| `PTH/AnimeSR-V2-4x` | AnimeSR | AnimeSR | 4x | 4 | 权重严格加载与描述器 |
| `PTH/AniScale2-DITN-i16-75K-2x` | DITN | DITN | 2x | 8 | 权重严格加载与描述器 |
| `PTH/AniScale2-ESRGAN-i16-110K-2x` | ESRGAN | ESRGAN / RRDB | 2x | 4 | 权重严格加载与描述器 |
| `PTH/AniScale2-ESRGAN-Lite-i16-165K-2x` | ESRGAN | ESRGAN / RRDB | 2x | 4 | 权重严格加载与描述器 |
| `PTH/AniScale2-Omni-i16-40K-2x` | OmniSR | OmniSR | 2x | 1 | 权重严格加载与描述器 |
| `PTH/AniScale2-Refiner-10K-1x` | RealESRGAN Compact | Compact | 1x | 1 | 权重严格加载与描述器 |
| `PTH/AniScale2-SwinIR-i16-265K-2x` | SwinIR | SwinIR | 2x | 1 | 权重严格加载与描述器 |
| `PTH/AniSD-AC-CRAFT-92500-2x` | CRAFT | CRAFT | 2x | 16 | 权重严格加载与描述器 |
| `PTH/AniSD-AC-G6i2a-Compact-72500-2x` | RealESRGAN Compact | Compact | 2x | 1 | 权重严格加载与描述器 |
| `PTH/AniSD-AC-G6i2b-SPAN-190K-2x` | SPAN | SPAN | 2x | 1 | 权重严格加载与描述器 |
| `PTH/AniSD-AC-G6i2b-SwinIR-117500-2x` | SwinIR | SwinIR | 2x | 1 | 权重严格加载与描述器 |
| `PTH/AniSD-AC-RealPLKSR-127500-2x` | RealPLKSR | RealPLKSR | 2x | 1 | 权重严格加载与描述器 |
| `PTH/AniSD-DB-i2-SPAN-85K-1x` | RealESRGAN Compact | Compact | 1x | 1 | 权重严格加载与描述器 |
| `PTH/AniSD-DC-CRAFT-127500-2x` | CRAFT | CRAFT | 2x | 16 | 权重严格加载与描述器 |
| `PTH/AniSD-DC-DAT2-97500-2x` | DAT | DAT | 2x | 1 | 权重严格加载与描述器 |
| `PTH/AniSD-DC-RealPLKSR-115K-2x` | RealPLKSR | RealPLKSR | 2x | 1 | 权重严格加载与描述器 |
| `PTH/AniSD-DC-SPAN-92500-2x` | SPAN | SPAN | 2x | 1 | 权重严格加载与描述器 |
| `PTH/AniSD-G6i1-SPAN-215K-2x` | SPAN | SPAN | 2x | 1 | 权重严格加载与描述器 |
| `PTH/AniSD-G6i1-SPAN-2x-215K` | SPAN | SPAN | 2x | 1 | 权重严格加载与描述器 |
| `PTH/AniSD-G6i1-SwinIR-165K-2x` | SwinIR | SwinIR | 2x | 1 | 权重严格加载与描述器 |
| `PTH/AniSD-G6i1b-Compact-102500-2x` | RealESRGAN Compact | Compact | 2x | 1 | 权重严格加载与描述器 |
| `PTH/AniSD-PS-G6i2-Compact-50K-2x` | RealESRGAN Compact | Compact | 2x | 1 | 权重严格加载与描述器 |
| `PTH/AniSD-RealPLKSR-140K-2x` | RealPLKSR | RealPLKSR | 2x | 1 | 权重严格加载与描述器 |
| `PTH/AniToon-RPLKSR-197500-2x` | RealPLKSR | RealPLKSR | 2x | 1 | 权重严格加载与描述器 |
| `PTH/AniToon-RPLKSRL-280K-2x` | RealPLKSR | RealPLKSR | 2x | 1 | 权重严格加载与描述器 |
| `PTH/AniToon-RPLKSRS-242500-2x` | RealPLKSR | RealPLKSR | 2x | 1 | 权重严格加载与描述器 |
| `PTH/APISR-DAT-GAN-generator-4x` | DAT | DAT | 4x | 1 | 权重严格加载与描述器 |
| `PTH/APISR-GRL-GAN-generator-4x` | GRL | GRL | 4x | 1 | 权重严格加载与描述器 |
| `PTH/APISR-RRDB-GAN-generator-2x` | ESRGAN | ESRGAN / RRDB | 2x | 4 | 权重严格加载与描述器 |
| `PTH/APISR-RRDB-GAN-generator-4x` | ESRGAN | ESRGAN / RRDB | 4x | 1 | 权重严格加载与描述器 |
| `PTH/BHI-SpanPlusDynamic-2x-Light` | sudo_SPANPlus | SPANPlus | 2x | 1 | 权重严格加载与描述器 |
| `PTH/ModernSpanimation-V2-2x` | SPAN | SPAN | 2x | 1 | 权重严格加载与描述器 |
| `PTH/ModernSpanimation-V3-2x` | SPANPlus | SPANPlus | 2x | 1 | 权重严格加载与描述器 |
| `PTH/Nomos8k-span-otf-4x-medium-NoUpdateParams` | SPAN | SPAN | 4x | 1 | 权重严格加载与描述器 |
| `PTH/Nomos8k-span-otf-4x-strong` | SPAN | SPAN | 4x | 1 | 权重严格加载与描述器 |
| `PTH/Nomos8k-span-otf-4x-weak-NoUpdateParams` | SPAN | SPAN | 4x | 1 | 权重严格加载与描述器 |
| `PTH/OpenProteus-Compact-i2-2x-70K` | RealESRGAN Compact | Compact | 2x | 1 | 权重严格加载与描述器 |
| `PTH/realesr-animevideov3` | RealESRGAN Compact | Compact | 4x | 1 | 权重严格加载与描述器 |
| `PTH/RealESRGAN-General-x4v3` | RealESRGAN Compact | Compact | 4x | 1 | 权重严格加载与描述器 |
| `PTH/RealESRGAN_x2plus` | ESRGAN | ESRGAN / RRDB | 2x | 4 | 权重严格加载与描述器 |
| `PTH/RealESRGAN_x4plus` | ESRGAN | ESRGAN / RRDB | 4x | 1 | 权重严格加载与描述器 |
| `PTH/RealESRGAN_x4plus_anime_6B` | ESRGAN | ESRGAN / RRDB | 4x | 1 | 权重严格加载与描述器 |
| `PTH/realesr-general-wdn-x4v3` | RealESRGAN Compact | Compact | 4x | 1 | 权重严格加载与描述器 |
| `PTH/Sudo-Shuffle-Span-2x-NoUpdateParams` | sudo_SPANPlus | SPANPlus | 2x | 1 | 权重严格加载与描述器 |

## 验证记录

- 首轮 25 个实际运行场景通过：重命名 PTH 导入、旧记录保留、非法参数拒绝、CUDA/TensorRT 图片与视频原生/2x/3x/4x、原生 Engine 复用、NCNN/ONNX/1x 修复与 BasicVSR++ 输出。
- 补充 12 个场景通过：包含 4 个基础场景复核，以及 NCNN 图导入/实际处理、重命名 ONNX、1x TensorRT、两种同后端组合顺序、跨后端组合和 FlashVSR 直接 2x。
- 界面 6 项状态探针通过：默认原生、两页保存同步、准确缩放提示、RTX 专用规格与原生提示；未显示窗口或启动宿主。
- 最终 8 项场景通过（含基础 4 项复核）：导入 1x 修复模型可见性、误导倍率的 ONNX 文件名、重命名 SwinIR 图、现有 crop 与 -s 预设下的最终目标尺寸。三轮去重共 37 个实际运行场景。

## 参考实现

- [Real-ESRGAN AnimeVideo 官方说明](https://github.com/xinntao/Real-ESRGAN/blob/master/docs/anime_video_model.md)：PTH 原生 4x；NCNN 的 2x/3x 导出图含后续缩放。
- [Real-ESRGAN 官方实现](https://github.com/xinntao/Real-ESRGAN/blob/master/inference_realesrgan.py)：Compact 与 RRDB 的模型构建分支。
- [Spandrel 官方接口说明](https://chainner.app/spandrel/spandrel.ImageModelDescriptor.html)：网络、倍率及输入要求描述器。本轮数值取自本机随项目后端分发的实现，不以在线最新版替代实际运行库。
