# PR #8 与 PR #9 本地审核

审核时间：2026-10-04 10:32；主线 `5942b48`。仅本地审核，不发送 GitHub 评论、不合并、不关闭 PR。

## PR #8：不建议整包合并

- [PR #8](https://github.com/maxzrb/VideoEnhancer/pull/8)，头提交 `cbaaedc`，基于旧 1.3.7（`ef7bee8`）。
- TensorRT 2/3/4x 输出问题已由当前 1.3.9 的原生倍率与目标输出倍率分离覆盖：`cli/OutputScale.cs` 和 `cli/Program.cs` 采用原生推理后 Lanczos 输出缩放，目标范围 1–8。PR 为同一模型新增三个伪预设，并在 Engine 内 bicubic 缩放、按输出倍率拆缓存，增加构建/缓存成本，也与现有倍率语义不同。不是原生模型变成三份不同权重。
- `cli/InstallerManager.cs` 的 RunInteractive 改回控制台输入 Y、选 EXE、创建目录确认及等待 Enter；`cli/InstallerBundle.cs` 保留控制台子系统，与用户已确认的 WiX 窗口交互方向冲突。
- aria2-next 2.8.3 及依赖许可整理有独立移植价值，但升级收益需要单独验证，不值得捆绑上述产品行为。
- 只读 merge-tree 预演返回冲突：Program.cs、VideoEnhancer.csproj、STATUS.md、发布流程、工作进度。不要用旧分支产物直接覆盖当前 1.3.9，或用整文件选 theirs 解决冲突。

## PR #9：有试用价值，原版不兼容当前本机 LakeUI

- [PR #9](https://github.com/maxzrb/VideoEnhancer/pull/9)，精确头提交 `ae94b26`。项目内 detached worktree：`Artifacts/pr9-review`。
- **部署阻碍**：本机 `C:/Program portable/3FUI/3FUI/FFmpegFreeUI.exe` 单文件包实际内嵌 FFmpegFreeUI 6.2.33.0 与 LakeUI 5.109.0.0；PR 固定编译 5.110.0，并在 `LakeScrollPanel.vb` 四处直接调用 `D3D_PaintBridge.BeginRenderUpdate`。`PluginPanel.vb`/UpscalePage 的版本门禁也明确阻止 5.109 加载页面。当前旧设备记录不能代表本机实际版本。
- 这是明确提升最低宿主基线，不应宣称兼容全部旧 5.x。原版试用须先有匹配宿主；兼容试用版须提供旧 API 处理且降低门禁，滚动新事务效果不能等同原版。
- 宿主解耦、96 DPI 基准和往返缩放、移除 JSON 布局、滚动统一提交与便携构建路径具有维护价值。
- 新 HostRuntime 以反射读写原始对象，定向测试与本机真实宿主类型/成员契约检查通过；真实编码、暂停/停止及预览整链仍需要实机验收。
- PR 自述压力录像仍有一帧背景偏移；现有布局测试不能证明所有背景滚动残影消除。

## 已执行验证

- 原版完整 `dotnet publish VideoEnhancer.slnx -c Release` 成功；插件与安装器构建 0 警告/0 错误，运行 EXE/安装器/ZIP 已生成。
- `ModelMetadataUi --host-runtime` 隔离行为与从本机单文件提取的真实宿主公开契约检查通过。
- `ModelMetadataUi --dpi` 通过 96/120/144/192 与缩放往返；`--scroll` 通过轮滚转发、动画、边界和无旧位图搬移检查。日志在 `Artifacts/pr9-review/Artifacts/pr9-{dpi,scroll}.log`。
- 以上测试使用 NuGet LakeUI 5.110；不把它们当成 5.109 宿主已能加载原版的证据。
- 已询问用户选择制作兼容试用版或先升级宿主；收到选择前不覆盖安装文件。

## 2026-10-04 10:46 续作：选择性aria2升级与组合部署

用户已升级3FUI至6.2.35/LakeUI5.110；PR8仅aria2-next2.8.3版本/固定哈希/源码与许可打包选择性移植到main，不合并倍率或安装交互。PR9精确ae94b26在Artifacts/pr9-review隔离组合publish通过，已备份后部署；EXE/DLL/aria2哈希一致，配置/用户能力清单保持。PR9未合并，未发布/提交，等待实际试用反馈。

官方2.8.3 tag与源码提交、二进制/源码hash核对通过；COPYING/AUTHORS原文及13项依赖许可通过（仅行末空白规范化差异），ZIP15项关键文件一致。aria2实际下载GitHub stable.json1665字节，SHA256与官方digest一致；组合publish0警告0错误，升级后真实宿主契约通过。

通过正式自更新部署CLI/DLL，独立aria2及许可另行显式更新；配置/用户能力清单保持。备份`C:/Codex Program/3fui plugin/Artifacts/.refactor-tmp/backup-before-pr9-aria2-20261004-104341`，证据`Artifacts/pr9-review/Artifacts/local-deployment.json`；未自动启动宿主。main选择性升级及记录未提交，PR9是否合并等待本机反馈。
