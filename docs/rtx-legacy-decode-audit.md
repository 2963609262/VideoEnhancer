# 老视频 RTX VSR 再次失败排查

更新时间：2026-10-04 23:49。用户原片、错误日志及是否单独更新 RTX 组件仍未知；以下为本地已复现问题，不能直接认定反馈同因。

## 已复现与修复

1. **奇数尺寸上传失败**：MPEG-4 Visual 343×259 可正常软解，但 NV12/P010 的 D3D11 纹理需要偶数宽高，原上传返回 `decoder_upload_buffer_failed / Cannot allocate memory`，实际不是已证明的内存耗尽。内部上传缓冲改为偶数尺寸，补齐边缘像素；GPU 源矩形仍取原可见尺寸，避免改变目标尺寸或新增黑边。完整 CLI 4x 输出 1372×1036、4 帧通过。
2. **软件解码帧格式未覆盖**：原转换支持常见平面 YUV，BGR24 和灰度样本返回 `software_decode_format_unsupported`。补充整数 RGB、调色板、灰度及描述符支持的打包/半平面 YUV 转换到 NV12/P010。RGB 转换匹配现有 D3D11 输入矩阵，并直接量化到目标位深；灰度补中性色度，不新增运行 DLL。颜色转换在 CPU 端完成，上传后 RTX 超分仍在 GPU 中执行。
3. **部署核对仍必要**：更新插件本体不等于更新 RTX 独立组件；已发布 2026.10.04.1 不含本轮奇数尺寸、格式补充和黑边修复。用户是否已更新组件仍待反馈。

新候选保留上一轮可见区域/crop 修复及首帧几何日志，并记录软解编码、像素格式和尺寸。没有修改超分模型权重。

## 验证

- `cli/tests/verify_rtx_legacy_decode.py` 11 样本通过：奇数尺寸 MPEG-4 Visual、XVID 非常见尺寸、MSMPEG4v3、WMV2、H263、MSVideo1/RGB555、PAL8、RV40、YUYV422、BGR24、灰度。检查实际软解上传路线、输出尺寸和帧数；带音频的生成样本检查复制后音频帧数。
- RV40 使用已有官方样本前缀，先无损封装四个视频包；仅证明这些 RV40 视频帧可以处理，不证明整段 RMVB、COOK 音频或损坏文件均通过。
- 原解码五样本与可见区域五样本回归通过，白色裁剪样本输出黑像素为零；C++ 单元测试 19 项通过，新增 RGB/调色板通道、灰度中性色度和 YUYV 色度平均检查。
- 完整 CLI 奇数尺寸 MPEG-4 4x 输出证据：`Artifacts/rtx-legacy-audit/cli-mpeg4-odd.log`。
- 最终实机证据：`Artifacts/rtx-legacy-audit/reusable-verification`、`decode-regression`、`border-regression` 及对应日志；单位验证 `unit-tests.log`。

运行示例（需要 RTX GPU 和隔离 runtime）：

```powershell
python cli/tests/verify_rtx_legacy_decode.py --runtime <隔离运行目录> --work <项目内产物目录> --rv40-sample <可选RV40源片>
```

## 源码与发行状态

源码工作副本：`Artifacts/rtx-backend-fix`，基准 6afb9a8。本次可审查完整候选补丁：[rtx-video-legacy-upload.patch](../release/patches/rtx-video-legacy-upload.patch)，包含上一轮黑边修复；与 `rtx-video-visible-rect.patch` 二选一，不能叠加应用。

隔离候选 EXE SHA256：`690b0801859eb625fb32b7706d378046777c6fe5363433983eac522413396983`。源码和补丁尚未提交、推送；未替换实际安装、未上传发行资产，仍遵守用户暂停发布要求。恢复发行时需要打包新 RTX 组件、校验组件更新识别，并完成远端回读和安装验证。未取得原反馈视频，未验收全部像素格式、驱动、长片或损坏视频。

## 1.3.12发行整合

用户已授权发布1.3.12，解除上述暂停。最终RTX源码提交2db5b02，独立版本2026.10.04.2，候选包含黑边与老视频上传两轮修复，进入发行验证；最终结果以STATUS最新快照为准。
