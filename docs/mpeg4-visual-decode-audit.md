# MPEG-4 Visual失败定位

核查时间：2026-10-04 19:47；实际安装插件1.3.10。用户提供源编码MPEG-4 Visual / XviD / DivX / FMP4，尚未提供失败后端与日志。

## 结论

没有证据表明模型拒绝MPEG-4 Visual编码。当前本机FFmpeg软解及普通NCNN模型超分均通过生成样本；当前RTX Video在普通尺寸FMP4样本失败，而低尺寸触发现有软解后成功。已复现RTX解码路由问题，但不能直接认定用户使用的就是该路径。

| 对照 | 输入 | 验证结果 |
| --- | --- | --- |
| FFmpeg软解，后备及宿主实际8.1.1 | FMP4/XVID/DIVX标记、mpeg4、640x360、4帧 | 全部成功，RGB24各2764800字节，与4帧尺寸一致 |
| 部署OpenCV探测 | 同上三样本 | 全部打开/首帧成功，640x360、24fps、4帧 |
| 普通NCNN，AVV3原生2x | FMP4 640x360 | CLI成功，输出1280x720、完整4帧；后端渲染完成，既有Vulkan退出阶段提示不影响此次输出完整性 |
| RTX VSR 2x | FMP4 640x360 | CLI退出1，处理0帧；FFmpeg could not send a packet to the decoder / Invalid argument |
| RTX VSR 2x，低尺寸对照 | FMP4 192x128 | CLI成功，输出384x256、完整4帧 |

生成样本使用FFmpeg mpeg4编码器和FMP4/XVID/DIVX FourCC，不代表覆盖全部历史XviD/DivX编码器的GMC/QPel/packed B-frame组合，也不是用户原片复现。实际处理输出只写项目Artifacts。

## 原因

用户随后确认失败任务为 RTX 超分。以下为已发行版本的原因；修复及实跑结果见文末。

已发行对应sidecar源码c83df0f的ffmpeg_transcode_pipeline.cpp，software_decode_fallback仅在width<320或height<240时为真；普通尺寸设置choose_d3d11_format，该回调在没有AV_PIX_FMT_D3D11可选时返回AV_PIX_FMT_NONE，没有按编码能力转软解。本机普通尺寸FMP4在首包报错，低尺寸相同编码通过，与这一路由一致。

参考：[发行对应RTX源码](https://github.com/maxzrb/RTXHDR-RTXVSR/blob/c83df0f/backend/src/video/ffmpeg/ffmpeg_transcode_pipeline.cpp)、[FFmpeg MPEG-4解码器](https://github.com/FFmpeg/FFmpeg/blob/n7.1/libavcodec/mpeg4videodec.c)。这是解码器路线限制，超分模型尚未获得输入帧，增加模型编码白名单无法修复。

普通模型与RTX组合需区分：CLI启用普通RVE或补帧的RTX组合会先生成HEVC中间文件；纯RTX VSR直接送原视频给sidecar，因此源编码问题最直接影响后者。未验证用户究竟启用哪种组合。

## 后续修复

sidecar应按解码器D3D11硬解能力选择路线，无可用硬解配置时CPU解码，再转NV12/P010并上传供RTX处理；对宣称支持但初始化失败的情况按具体失败点安排重新打开软件解码器。现有软解转换仅处理yuv420p/yuv420p10le，其他像素格式的处理需明确，不以仅扩大尺寸条件冒充通用回退。此修复属于独立RTX runtime，须遵循其版本/分发流程。

需要用户失败后端（是否RTX VSR/HDR）、日志及原片编码/像素格式/尺寸以确认实际反馈；当前已发起澄清问题。

本轮仅调查，无产品代码修改、版本变更、发布或部署。样本、源码副本与结果在Artifacts/feedback-decode-audit，含mpeg4-decode-results.json、mpeg4-pipeline-results.json及三次实际超分日志，不入Git。

## 修复与验收（2026-10-04 21:17）

上述“仅调查”为19:47时的状态。用户已确认 RTX 超分并授权修复，现在独立组件源码已修改、构建并在隔离目录完成实跑；尚未发布或覆盖真实安装。

实现先枚举 `avcodec_get_hw_config` 的 D3D11VA 能力；不提供硬解配置的 MPEG‑4 Visual 直接软件解码。硬解初始化或首帧解码失败时重建软件解码器，保留并重放首帧探测读出的所有流数据包，再上传 NV12/P010 给 RTX。常见平面422/444及高位深输入补齐转换，保留原低尺寸回退。双显卡 RTX/NVENC 能力检查同步优先选 NVIDIA，避免探测默认集显而拒绝启动。

RTX 3060 Laptop 实机：FMP4 640x360、H.264 High10、H.264 444、H.264 192x128均成功软解并上传；普通 H.264 640x360仍走D3D11VA。五种都输出完整4帧，音频解码帧数与源文件一致。最终完整CLI入口对原调查FMP4样本输出1280x720/4帧成功。证据在 `Artifacts/rtx-decode-fix-verification/results.json`、`capabilities.json`、`VSR/logs/vsr_backend.log` 及 `Artifacts/model-audit/verification/results-final-fix.json`。

可审查源码差异为 [RTX补丁](../release/patches/rtx-video-decode-fallback.patch)，基于 `rve-patches/c83df0f`；[应用说明](../release/patches/README.md)。构建及候选组件ZIP在 `Artifacts/rtx-backend-fix` 和 `Artifacts/model-fix-build/rtx-video-runtime.zip`。软件解码仍取决于 FFmpeg 是否包含相应解码器及输入是否有效，当前不承诺覆盖所有历史编码器变体或损坏文件；本轮未取得用户原片。


## 2026-10-04 21:30 本机试用部署

用户授权后已部署到3FUI：CLI/插件DLL/RTX组件及配套脚本16项hash核对通过，保护配置和用户模型清单保持，已备份。部署后的实际CLI再次通过FMP4 RTX2x输出1280x720/4帧。证据及备份路径见Artifacts/model-fix-build/local-deployment.json；等待用户试用反馈，未发布远端。
