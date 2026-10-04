# 超分目标倍率处理路径审计

核查时间：2026-10-04 19:37。主线：0664186 / 1.3.10。依据本地源码及已部署后端副本，未进行逐模型GPU速度测试。

下文路径表记录修复前状态；本轮实施及验收见文末“修复结果”。

## 结论与范围

内置能力清单共98项，其中41项声明支持TensorRT：原生4x共11项、2x共28项、1x共2项。计数是清单声明，不代表本轮逐项GPU验收。

TRT目标倍率问题来自通用调用路径，不是AVV3专用分支。固定倍率模型选择低于原生的输出时，主线将原生倍率传给Engine构建，再在输出端缩放。部署转换脚本本已支持在Engine内加入最终bicubic缩放。原生倍率、Engine输出倍率和目标输出倍率应分开维护；不需要修改权重倍率或根据改名猜测模型能力。

## 各路径

| 路径 | 当前目标倍率处理 | 风险与边界 |
| --- | --- | --- |
| 普通TRT视频 | 原生倍率Engine→原生帧回传/管道→FFmpeg Lanczos目标缩放 | 所有支持该转换流程的固定倍率模型都需检查低倍率图内缩放；4x→2x传出像素量为2x输出的4倍，不能等同于总耗时或模型计算量4倍 |
| TRT图片 | 原生倍率Engine→原生RGB数组→Pillow Lanczos | 与视频同类风险，不能只修视频入口 |
| CUDA/NCNN/ONNX普通视频 | 模型原生推理→FFmpeg目标缩放 | 共用后处理位置，但不据此认定具有TRT旧版图内缩放的相同回归；应检查后端原有缩放方式及实际传输开销 |
| CUDA/NCNN/ONNX图片 | 原生推理→Pillow目标缩放 | 属原生后缩放，是否优化需按后端决定 |
| 同后端先超分再补帧 | 补帧初始化按upscaleTimes分配尺寸，目标输出滤镜在最终编码处 | 固定4x模型选择2x时，补帧仍可按4x尺寸运行；恢复前端缩放须同步帧尺寸、场景检测、补帧缓存 |
| 跨后端组合 | 两阶段视频，最终编码处目标缩放 | 必须检查中间文件实际倍率和第二阶段尺寸，不能只改变最终滤镜 |
| 分段模型视频 | 固定倍率模型要求一致；TRT按模型倍率构建 | 不使用普通-output-scale任意改变分段倍率；本轮不将此当作已复现的低倍率回归 |
| FlashVSR | 能力清单声明[2,4]；选择2x/4x可直接传推理倍率 | 原生多倍率能力已显式处理，不应误替换为固定4x模型的后缩放策略 |
| 原生1x恢复模型 | 目标大于1x时属于额外放大 | 不存在低于原生的正整数倍率，不能减少成另一种原生推理倍率 |
| RTX Video | 独立sidecar | 不经过此TRT模型转换入口，应独立核查 |

## TRT逐项范围

下面是清单声明的候选范围；风险触发需要用户选择低于原生的目标倍率，原生默认输出不因本项而新增一次低倍率缩小。明确设置相同倍率时仍会追加FFmpeg scale滤镜，是否产生额外耗时需测量，不能仅凭代码判定。

| 模型 | 原生倍率 | 涉及的低倍率目标 |
| --- | --- | --- |
| PTH/Ani4K-Compact-35000-2x | 2x | 1x |
| PTH/AnimeJaNai-V2-2x-Compact-36K | 2x | 1x |
| PTH/AnimeJaNai-V3-2x-HD-Sharp1-Compact-430K | 2x | 1x |
| PTH/AniScale2-DITN-i16-75K-2x | 2x | 1x |
| PTH/AniScale2-ESRGAN-i16-110K-2x | 2x | 1x |
| PTH/AniScale2-ESRGAN-Lite-i16-165K-2x | 2x | 1x |
| PTH/AniScale2-Omni-i16-40K-2x | 2x | 1x |
| PTH/AniScale2-Refiner-10K-1x | 1x | 无 |
| PTH/AniSD-AC-G6i2a-Compact-72500-2x | 2x | 1x |
| PTH/AniSD-AC-G6i2b-SPAN-190K-2x | 2x | 1x |
| PTH/AniSD-AC-RealPLKSR-127500-2x | 2x | 1x |
| PTH/AniSD-DB-i2-SPAN-85K-1x | 1x | 无 |
| PTH/AniSD-DC-DAT2-97500-2x | 2x | 1x |
| PTH/AniSD-DC-RealPLKSR-115K-2x | 2x | 1x |
| PTH/AniSD-DC-SPAN-92500-2x | 2x | 1x |
| PTH/AniSD-G6i1-SPAN-215K-2x | 2x | 1x |
| PTH/AniSD-G6i1-SPAN-2x-215K | 2x | 1x |
| PTH/AniSD-G6i1b-Compact-102500-2x | 2x | 1x |
| PTH/AniSD-PS-G6i2-Compact-50K-2x | 2x | 1x |
| PTH/AniSD-RealPLKSR-140K-2x | 2x | 1x |
| PTH/AniToon-RPLKSR-197500-2x | 2x | 1x |
| PTH/AniToon-RPLKSRL-280K-2x | 2x | 1x |
| PTH/AniToon-RPLKSRS-242500-2x | 2x | 1x |
| PTH/APISR-DAT-GAN-generator-4x | 4x | 1x, 2x, 3x |
| PTH/APISR-GRL-GAN-generator-4x | 4x | 1x, 2x, 3x |
| PTH/APISR-RRDB-GAN-generator-2x | 2x | 1x |
| PTH/APISR-RRDB-GAN-generator-4x | 4x | 1x, 2x, 3x |
| PTH/BHI-SpanPlusDynamic-2x-Light | 2x | 1x |
| PTH/ModernSpanimation-V2-2x | 2x | 1x |
| PTH/ModernSpanimation-V3-2x | 2x | 1x |
| PTH/Nomos8k-span-otf-4x-medium-NoUpdateParams | 4x | 1x, 2x, 3x |
| PTH/Nomos8k-span-otf-4x-strong | 4x | 1x, 2x, 3x |
| PTH/Nomos8k-span-otf-4x-weak-NoUpdateParams | 4x | 1x, 2x, 3x |
| PTH/OpenProteus-Compact-i2-2x-70K | 2x | 1x |
| PTH/realesr-animevideov3 | 4x | 1x, 2x, 3x |
| PTH/RealESRGAN-General-x4v3 | 4x | 1x, 2x, 3x |
| PTH/RealESRGAN_x2plus | 2x | 1x |
| PTH/RealESRGAN_x4plus | 4x | 1x, 2x, 3x |
| PTH/RealESRGAN_x4plus_anime_6B | 4x | 1x, 2x, 3x |
| PTH/realesr-general-wdn-x4v3 | 4x | 1x, 2x, 3x |
| PTH/Sudo-Shuffle-Span-2x-NoUpdateParams | 2x | 1x |

## 修复方案与验证重点

1. 通用TRT转换入口分开传原生倍率与Engine输出倍率，低目标倍率用现有FinalOutputScale图内后缩放；保留按输出倍率隔离缓存。倍率不匹配的旧Engine不得当成新目标复用。
2. 视频和图片同步处理，引擎实际输出尺寸用于帧数组及预览；Engine已输出目标尺寸时避免重复缩放。
3. 分块需按Engine实际输出倍率拼接/裁边；组合补帧需同步初始化尺寸和处理顺序，不能只修AVV3名称判断。
4. 明确bicubic图内缩放与当前Lanczos的画质差异，不承诺同算法或相同画质。目标高于模型原生倍率仍需要另外放大；FlashVSR真实多倍率保留。
5. 仅进行代表性低倍率TRT视频/图片、分块、组合补帧、原生输出缓存与尺寸检查，按具体变更补充，不扩大到全部模型重测。旧新性能对比需相同输入、精度、分块、编码、补帧设置及预热，排除Engine首次编译。

本轮完成的是路径审计，尚未实施产品代码修复或实測各模型速度。

## 代码依据

- [内置能力清单](../cli/model-capabilities.json)
- [视频/图片任务、Engine构建及缓存](../cli/Program.cs)：视频requestedScale、RunImageJob、EnsureTensorRtEngine、BuildTensorRtCachePath。
- [编码端缩放](../cli/OutputScale.cs)
- [图片后缩放](../cli/embedded-tools/rve-image-backend.py)
- [组合顺序和补帧尺寸](../cli/embedded-tools/rve-ordered-backend.py)
- [分段桥](../cli/embedded-tools/rve-segmented-backend.py)
- 部署后端convert_tensorrt.py的FinalOutputScale已核对副本hash一致，具体证据见STATUS前一会话；该后端脚本不在主线源码目录，后续修改需遵循独立后端分发方式。

## 2026-10-04 19:40 全后端补充核查

范围应覆盖全部后端，不只TRT。下表统计按内置清单声明，后端可重叠（CUDA和TRT共用PTH），不能相加作为唯一模型数；未知用户模型按实际检测和输出路径归类。

| 后端 | 清单数量及登记倍率分布 | 低倍率视频当前路径 | 优化位置与边界 |
| --- | --- | --- | --- |
| CUDA/PyTorch | 47：1x 2、2x 33、4x 12 | 原生推理→CPU原生RGB→FFmpeg缩小 | 在张量回CPU前缩小可减少传输；需保留模型真实倍率、分块拼接和时序上下文，不能把整个模型原生推理改为低倍率 |
| NCNN/Vulkan | 21：1x 2、2x 13、3x 1、4x 5 | 原生输出bytes→FFmpeg缩小 | 现有封装返回bytes，先在RVE缩小可减少编码管道量；是否在Vulkan内缩小要看封装能力，不能承诺减少GPU→CPU传输 |
| ONNX Runtime | 28：1x 2、2x 23、4x 3 | 原生推理→numpy/RGB bytes→FFmpeg缩小 | 与NCNN同样可先在帧管道缩小；GPU侧优化需图/输出绑定支持，不能把任意ONNX尺寸或图倍率改掉 |
| FlashVSR | 1：声明原生4x并显式支持2/4x | 2x/4x直接传node.main的scale；其他目标在编码端缩放 | 保留真实多倍率；选择1x/3x仍需后缩放策略 |
| BasicVSR++ | 清单1项官方4x；优化目录代码另识别1x | 模型原生片段输出→numpy→RawVideoWriter→FFmpeg | 官方4x低目标仍有后缩放开销，需在时序片段输出端处理；1x目录不是把4x权重改名 |
| RTX Video | 独立sidecar，不在上述模型清单 | 独立输出规格 | 不使用普通-output-scale入口，本项不据此推断RTX也存在相同回归 |

与旧版的区别：v1.3.8接受覆盖倍率并传--override_upscale_scale，普通RVE RenderVideo在输出队列前调用resize_image_bytes，缩小时使用OpenCV INTER_AREA；新版目标倍率进入FFmpeg滤镜，模型覆盖值仍为原生倍率。因此CUDA/NCNN/ONNX也有缩放位置与算法变化，不能再仅描述为TRT风险。普通旧路径CPU缩小前同样已有原生帧回CPU，本轮没有证据证明这三类旧版都在GPU内缩放。TRT图内缩放另有转换脚本支持。

先超后补尤需统一：旧组合桥在超分后、补帧前执行override尺寸调整并按目标尺寸初始化补帧；当前目标只作用在最终编码，同后端补帧和跨后端中间视频可仍为原生高尺寸。4x推理目标2x时，补帧输入面积为目标尺寸的4倍，不能等同总速度下降4倍。先补后超则不出现这项高尺寸补帧开销。

图片同样要处理：通用图片桥在原生结果转RGB数组后用Pillow Lanczos缩放；FlashVSR 2x/4x会直接传真实scale，BasicVSR++构造短视频仍按模型原生结果提取后缩放。

统一修复应管理模型原生倍率、处理链当前帧尺寸及最终目标尺寸：在超分结果产生后、需要最终尺寸的后续阶段前只缩放一次；GPU输出可用时尽量在回CPU前缩放，bytes/numpy后端先减少后续管道量。各算法与缩放位置需明确，不能把原Lanczos静默替换为bilinear，也不能宣称各模型存在隐藏多倍率。分块/组合、预览/缓存应同步。此处为方案审计，未修改产品代码或实测速度。

## 修复结果（2026-10-04）

- TRT 视频、图片的低目标倍率传给已有 `FinalOutputScale`，恢复图内双三次；缓存仍以实际输出倍率隔离。AVV3 4x 权重生成实际输出 2x 的 Engine，不改变原生能力检测。
- 新增结果端尺寸模块。CUDA 张量和 BasicVSR++ 片段使用原设备上的可分离 Lanczos4，缩放后才回传；不依赖 `interpolate` 的双三次替代。采样坐标、边界和权重经 OpenCV Lanczos4 浮点参考验证，支持 FP16 和不同长宽。该算法与 FFmpeg/Pillow 的 Lanczos 参数、抗锯齿实现并非逐像素相同。
- NCNN、ONNX 当前封装返回 CPU bytes/numpy，在进入下游队列前使用 CPU Lanczos4。FlashVSR 保留真实 2x/4x 推理，其他目标在写编码器前处理；其原有分块合并位于 CPU，本轮未把大时间窗口画布搬入显存。没有承诺这些封装的 GPU→CPU 传输已消除。
- 同后端两种补帧顺序及跨后端中间文件均落实目标尺寸；先超后补按目标尺寸初始化插帧器。先补后超复制转场重复帧，避免超分原地改写源帧或插帧缓存。编码端保留最终尺寸兜底，输入尺寸相同时无额外尺寸变化，用户裁剪等滤镜仍能保证最终目标尺寸。
- 代表性实跑通过 CUDA/TRT 4x→2x、NCNN 2x→1x、ONNX 2x→3x、BasicVSR++ 4x→2x、FlashVSR 2x/3x、两种 RIFE 顺序、NCNN→CUDA 组合、CUDA/TRT 图片及 2x Engine 缓存；完整结果在项目忽略目录 `Artifacts/model-audit/verification/results-scale-fix.json`。不是全98模型逐项验收。
- RTX 3060 Laptop 单独测量 GPU Lanczos4：4K FP16→1080p，预热3次后10次平均11.793ms，进程峰值分配314,154,496字节；FP16结果相对CPU FP32参考最大绝对误差0.0004884。证据 `gpu-lanczos-benchmark.json`；不是超分整链测速，不宣称恢复用户旧版全部速度。
- CLI/插件候选产物在 `Artifacts/model-fix-build`，保持开发版本1.3.10；未部署到真实安装或发布。独立RTX组件需随源码补丁单独交付。
- 最终追加验证通过 TRT 32像素分块输出2x、TRT目标6x的GPU结果放大、修复后的两种RIFE顺序及完整RTX CLI入口。四帧素材在2倍补帧时为4个源帧加3个间隙帧，共7帧，与RVE相邻帧生成规则一致；未把进度预估8帧当作实际写帧数。结果在 `results-final-fix.json`。
- [PyTorch interpolate官方接口](https://docs.pytorch.org/docs/2.14/generated/torch.nn.functional.interpolate.html)未提供Lanczos模式；这仅是现用接口的限制，GPU缩放模块用原设备张量运算实现Lanczos4，不据此声称GPU只能使用双三次。


## 2026-10-04 21:30 本机试用部署

用户授权后已部署到3FUI：CLI/插件DLL/RTX组件及配套脚本16项hash核对通过，保护配置和用户模型清单保持，已备份。部署后的实际CLI再次通过FMP4 RTX2x输出1280x720/4帧。证据及备份路径见Artifacts/model-fix-build/local-deployment.json；等待用户试用反馈，未发布远端。


## 2026-10-04 21:47 AVV3原生网络与NCNN输出图倍率澄清

官方[动漫视频模型说明](https://github.com/xinntao/Real-ESRGAN/blob/master/docs/anime_video_model.md)将AVV3标为X4，并说明可用于X1/X2/X3；[推理代码](https://github.com/xinntao/Real-ESRGAN/blob/master/inference_realesrgan.py)构造upscale=4/netscale=4的SRVGGNetCompact，[后处理](https://github.com/xinntao/Real-ESRGAN/blob/master/realesrgan/utils.py)在outscale不同于网络倍率时用CPU OpenCV Lanczos4缩放。官方公开这份PTH没有独立原生2x权重选择。TRT图内双三次是本项目已有转换器实现，不能说具体算法由官方指定。

纠正之前NCNN“原生2/3/4x”的表述：本项目附带的RealESRGAN-AnimeVideoV3-2x.param、3x.param末层卷积均48通道，PixelShuffle 0=4，再分别Interp 0.5/0.75；4x.param没有末端缩小。其图输出倍率确为2/3/4，学习网络仍4x。官方NCNN README的-s 2/3/4以及选择-x2/-x3/-x4图，也不能独自证明有独立2x/3x训练网络。现有按实际图输出尺寸运行和缓存的修复不因此失效，但能力清单/界面中“原生倍率”与“图输出倍率”的用词需要后续复核。此次只澄清和更新记录，未改部署中的代码或模型。
