# 模型悬停介绍依据

核对日期：2026-10-01。文案位于 `VideoEnhancerPlugin/ModelDescriptionProvider.vb`。

介绍优先说明素材类型、所处理的问题、版本差异与选择限制；模型名称、原生倍率和当前可用后端另列，不重复堆砌。下表是文案来源，不是本项目的统一画质或性能测试。作者的比较结论只适用于其模型和测试条件。

| 模型或系列 | 公开依据 | 文案使用的信息 |
| --- | --- | --- |
| AnimeJaNai HD/SD、V2/V3 | [作者说明](https://github.com/the-database/mpv-AnimeJaNai/blob/main/README.md) | HD 针对制作分辨率放大造成的模糊；V3 对线色、景深和阴影的处理；SD 为标清训练 |
| AnimeJaNai V3.1 Balanced/Performance、Sharp1 | [3.3.0 发布说明](https://github.com/the-database/mpv-AnimeJaNai/releases/tag/3.3.0) | Balanced/Performance 的速度取舍、Standard/Sharp 版本；不移植播放器实时显卡门槛 |
| AniSD 基础/AC/DC/DB/PS | [作者的套件文档](https://github.com/Sirosky/Upscale-Hub/wiki/✨-AniSD-Suite-Documentation-‐-Faithful-Anime-DVD-Restoration) | 标清 DVD/WEB；基础、增强清理、深度清理；DB 去色渗、PS 保持原貌放大；DAT 的开销与色偏限制 |
| AniScale2 各网络、Refiner | [作者的模型文档](https://github.com/Sirosky/Upscale-Hub/wiki/🌟-AniScale-2-&--AniScale-2-Refiner) | OmniSR 为作者综合推荐；DITN 的景深/细节限制；Refiner 放大前修线、放大后细线及对景深的影响 |
| AniToon Small/标准/Large | [作者发布页](https://github.com/Sirosky/Upscale-Hub/releases/tag/AniToon) | 老卡通/动画复原、不同网络大小；作者报告的 VSMLRT 偏差只写明对应环境，不断言本项目 TensorRT 不可用 |
| OpenProteus | [作者发布页](https://github.com/Sirosky/Upscale-Hub/releases/tag/OpenProteus) | 高清低噪声实拍影视，忠实放大；纠正旧文案“动漫/插画”定位。页面正文另外经 GitHub Releases API 读取核对 |
| Ani4K Compact | [作者发布页](https://github.com/Sirosky/Upscale-Hub/releases/tag/Ani4K) | 高清动画细节保留；不写成针对严重 DVD 压缩的模型 |
| Nomos8k OTF strong/medium/weak | [作者上传的模型卡](https://openmodeldb.info/models/4x-Nomos8k-span-otf-strong)、[medium](https://openmodeldb.info/models/4x-Nomos8k-span-otf-medium)、[weak](https://openmodeldb.info/models/4x-Nomos8k-span-otf-weak) | 主要用于照片，按退化强弱选择；不误归为动漫专用 |
| RealESRGAN x2plus/x4plus/anime 6B、General/WDN | [官方模型清单](https://github.com/xinntao/Real-ESRGAN/blob/master/docs/model_zoo.md)、[官方实现](https://github.com/xinntao/Real-ESRGAN/blob/master/inference_realesrgan.py) | 实拍/动漫用途、6 块网络、General 的轻量网络、WDN 的弱去噪插值作用 |
| AnimeVideo v3 | [官方模型说明](https://github.com/xinntao/Real-ESRGAN/blob/master/docs/anime_video_model.md)、[NCNN 加载实现](https://github.com/xinntao/Real-ESRGAN-ncnn-vulkan/blob/master/src/main.cpp) | 动画用途；逐帧处理；PTH 原生 4x 与独立 NCNN 2x/3x/4x 图的区别 |
| Real-CUGAN Conservative | [官方说明](https://github.com/bilibili/ailab/blob/main/Real-CUGAN/README.md) | 保守版本减少画风/纹理改变，但对重度模糊的修复有限 |
| Waifu2x | [作者仓库](https://github.com/nagadomi/waifu2x) | 动漫/照片训练及去噪用途；Noise 等级不等同于所有素材上的质量等级 |
| DnCNN ColorBlind | [作者仓库](https://github.com/cszn/DnCNN) | 彩色盲高斯去噪；不误写成任意噪声都能自动修复 |
| DenoiseH264 | [上游 RVE](https://github.com/TNTwise/REAL-Video-Enhancer/blob/v2-main/README.md) 与本机模型清单 | 仅说明 H.264 压缩清理和实际 1x 能力；未找到完整训练说明，不扩展到所有压缩/噪声 |
| APISR RRDB/DAT/GRL | [官方仓库](https://github.com/Kiteretsu77/APISR)、[CVPR 论文](https://openaccess.thecvf.com/content/CVPR2024/papers/Wang_APISR_Anime_Production_Inspired_Real-World_Anime_Super-Resolution_CVPR_2024_paper.pdf) | 动画制作退化、手绘线条与色彩处理；架构是不同网络，不是简单质量档位 |
| AnimeSR | [官方仓库](https://github.com/TencentARC/AnimeSR) | 动画视频时序复原；当前仅 CUDA 来自本项目能力清单 |
| BasicVSR++ | [官方 BasicSR](https://github.com/XPixelGroup/BasicSR)、本项目后端实现 | 前后帧视频复原；本项目不与补帧组合，不沿用旧误写的单帧定位 |
| FlashVSR | [官方仓库](https://github.com/OpenImagingLab/FlashVSR)、本项目后端实现 | 单步扩散式时序超分；直接 2x/4x 来自当前实现；删除“不能参与补帧组合”的过时文案 |
| RIFE | [官方仓库](https://github.com/hzwer/ECCV2022-RIFE) | 双帧中间帧估计；删除无依据的 v4.25/v4.26 画质排名 |
| GMFSS | [作者仓库](https://github.com/98mxr/GMFSS_Fortuna) | 运动/特征/softsplat 融合及不同模型配置；不再声称 Union 利用更多输入帧 |
| GIMM F/R/LPIPS | [官方说明](https://github.com/GSeanCDAT/GIMM-VFI#GIMM-VFI-Models) | F 使用 FlowFormer，R 使用 RAFT；LPIPS 版增加感知损失训练 |

## 尚待复核

- RealHatGAN 的 JP/Universal 与 RealESRGAN JP Illustration 的 fix1/fix2：现有能力和真实结构已审计，但未取得足以解释训练及画质差异的作者资料。介绍明确待复核，不把编号当画质等级。
- ModernSpanimation V2/V3：上游有模型记录，实际结构分别为 SPAN/SPANPlus；尚未核实作者对两版权重画质差异的解释，删除“更新所以更好”一类表述。
- BHI SpanPlus Dynamic、Sudo Shuffle：保留已检测的结构和能力，不推断去噪强度或“避免 GAN 纹理”等训练特性。
- 未匹配公开模型的导入权重：只提供检测元数据，不因 Compact、ESRGAN 或 SwinIR 架构名称推断其训练题材。

逐项生成的 98 项实际文案可由 `cli/tests/ModelMetadataUi` 的 `--tooltips` 探针导出到 `Artifacts/model-audit/model-introductions.json`。此输出为本地验证证据，不替代上述公开来源。
