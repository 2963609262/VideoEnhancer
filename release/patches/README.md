# RTX Video 解码回退补丁

`rtx-video-decode-fallback.patch` 基于 [RTXHDR-RTXVSR](https://github.com/maxzrb/RTXHDR-RTXVSR) 的 `rve-patches` 分支，基准提交 `c83df0f`。此目录保留可审查的源码差异；修复源码提交 `6afb9a8`，组件版本 `2026.10.04.1`，随 1.3.11 分发。

在该源码仓库根目录执行 `git apply rtx-video-decode-fallback.patch`，再按上游 CMake 流程启用 `VSR_ENABLE_FFMPEG` 和 `VSR_ENABLE_RTX_SDK` 构建。

修改包括：枚举 FFmpeg 解码器的 D3D11VA 配置；硬解初始化或首帧探测失败时重新打开软件解码器；将软件解码帧上传 D3D11 后继续 RTX VSR/HDR；重放探测时读出的所有流数据包，保留起始音视频和字幕；补齐常见平面 YUV 422/444 及高位深到 NV12/P010 的转换。原低分辨率回退仍保留。

RTX/NVENC 能力探测与处理链一致，明确优先选择 NVIDIA 适配器，避免双显卡设备的默认集显造成启动前误判。

本地构建和证据在忽略目录 `Artifacts/rtx-backend-fix`、`Artifacts/rtx-decode-fix-verification`。RTX 实机回归脚本为 `cli/tests/verify_rtx_decode_fallback.py`，需隔离运行目录中的组件、FFmpeg 和 NVIDIA RTX GPU；脚本将测试视频及日志保存在指定 `--work` 目录。

```powershell
python cli/tests/verify_rtx_decode_fallback.py --runtime <隔离运行目录> --work <项目内测试产物目录>
```
