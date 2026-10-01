# Project Status

Last updated: 2026-10-01 13:06
Updated by: Codex

本文件是唯一 AI 操作状态来源。历史原文已冻结归档至 [记录归档](../archive/records-2026-10-01/README.md)，仅查历史时读取；归档中的版本、远端、环境和 TODO 不代表当前状态。

## Current Snapshot

- 用户最新指令：按发布流程正式发布 **1.3.8**，已授权版本更新、提交、推送与双源发布。当前发行构建和本地门禁已通过，正在发布；线上 1.3.7 不覆盖。
- 1.3.8 包括本日下载/解压/缓存/WiX全部改动，用户此前已反馈可用。后端29,709文件逐项审计 UNCHANGED，继续使用2026.09.30.1，不生成空更新。
- 教程缓存位于 `Plugin/videoenhancer/cache/TutorialImages`，迁移已知旧图片缓存，保留冲突/无关文件。下载完整异常进 `logs/downloads.log`，文件部署失败进 `logs/installer.log`；错误提示样式与五秒显示时长保持。
- 7z 使用便携 7-Zip 26.03 x64 多线程解压，SharpCompress 先做归档路径/链接预检，原生 CRC 保留；单运行 EXE 内含工具、许可证和对应源码。界面仅显示解压百分比，不显示文件名；取消轮询只在任务有标记时启用。
- 下载全部先单独完成后端安装，再按列表顺序最多三个模型任务并行、持续补位。取消或普通失败不阻断队列；离线/认证问题停止补位并等当前任务结束。手动重试加入队尾，重复点击去重，有空位立即开始，按文件最新状态统计；不自动重试。
- 下载全部运行时可点击停止全部，停止补位并取消当前可取消任务；后端替换事务不强杀，安全结束后不启动模型队列。分类下载采用相同队列规则。Windows Job 负责宿主退出后的下载子进程清理。
- 保留现有 WiX Burn 6.0.2 窗口；链仅含 Permanent EXE，Cache=remove，默认 Burn 日志关闭，/log 诊断保留。实测正常成功/正常失败后无本次载荷缓存、Bundle 缓存、临时卸载和依赖登记。不是 MSI；旧缓存及异常强杀/断电、Windows 自身记录不保证清零。

## Active TODO

- [ ] 完成1.3.8源码提交/标签、GitHub发布、ModelScope镜像及远端回读核验，完成后提交收尾记录。
- [ ] 维护清理：本机旧版三份载荷缓存约 59 MB；项目三份完整解压验证输出约 18 GB。后者曾两次被自动审批以 blocked by policy 拒绝，未绕过。确认归属与路径后再单独处理，勿清系统共享缓存。
- [ ] 历史发布验证补充：1.3.7 的 GitHub 大资产此前 CDN 下载超时，仅 API digest/大小核对；ModelScope 实际下载哈希通过。网络恢复后可补 GitHub 大资产实际下载校验。
- [ ] 长期可选工作：发行自动化与上游选择性同步清单；模型镜像逐文件来源/授权审计。项目自身 MIT 已落实，不沿用旧的“项目许可证未定”说法。
- [ ] 遗留专项验收记录：高 DPI/模型菜单/预览压力/四宫格完整交互、真实 TensorRT 缓存及组合视频专项在旧记录中未全部补最终验收；后续相关修改时定向复验。本次用户反馈仅关闭本轮下载交互验收，不代表全部历史专项完成。

旧预览目录、已发布 1.1.0、已合并 PR 和已被新后端取代的验证任务已退出当前 TODO，原始上下文保留在归档；不得按旧记录恢复到预览主线或旧后端。

## Recently Completed

- 2026-09-30：正式发布 1.3.7，教程图片滚动缓存优化；GitHub/ModelScope 发布记录已提交并推送。
- 2026-10-01：便携教程缓存、原生解压、取消和日志、WiX 安装残留收敛、解压百分比显示、持续补位与手动重试入队完成；本机已更新，用户反馈无明显问题。
- 2026-10-01：当前操作记录压缩、完整原文归档、导航更新；未改程序或版本。

## Decisions

- 当前根目录 main 是唯一开发主线；origin 为独立维护仓库，upstream 仅供选择性移植。不沿用旧 fork/origin 名称或 preview 工作目录。
- 保留 LakeUI 界面和当前 WiX 安装器；用户明确拒绝已有便携自解包交互窗口。
- 后端体积敏感，先单独安装；模型数量敏感，使用三并发滑动窗口。单项取消与停止全部分开，失败/取消不自动重试，手动重试可加入运行队列。
- 错误完整信息写插件日志；不改五秒错误提示，不展示解压文件名。减法针对运行期查询/轮询/输出开销，不宣称安装包更小。
- 保留后端增量路由、完整修复确认和事务回滚；半成品不标为安装成功，取消保留可重试缓存/断点。
- 项目 MIT 与第三方各自许可证并存；Aria2 Next Release 源码资产供许可证履行，普通用户不用另外安装该源码包。
- 跨后端/模型能力、转场阈值和分块语义以当前源码为准；历史被推翻的决定仅查归档，不从旧快照恢复默认值。

## Risks And Blockers

- 当前版本源已递增1.3.8，发行构建/包清单一致；发布进行中，双源回读完成前不宣告结束。
- 用户最初的 99% 五分钟后 IO/end 类失败未复现；原实现同包最终成功，不能把根因写成已确证。再次发生时读取插件日志。
- 原生工具/源码内嵌增加运行 EXE 约 2 MB；优化结果是解压耗时与运行开销降低，不是文件体积下降。
- 历史“无 NVIDIA/缺宿主程序集/只隐藏控制面板”等过时阻塞已移出当前状态；当前设备曾完成真实 GPU 验证，本轮也已有可用构建引用。换设备后仍须重查。
- 旧清理拒绝和 GitHub CDN 验证限制见 Active TODO；不将历史注册残留推测当实测事实。

## Environment Notes

以下仅为本机已验证路径，不能当作跨设备配置。

- 工作区：`C:/Codex Program/3fui plugin`；Windows / PowerShell，.NET 10，文件读写 UTF-8。
- 本轮构建 HostBin：`C:/Users/maxzr/AppData/Local/Temp/3fui-core-compat-host`，LakeUI 5.9。实际宿主发布根不含可用 FFmpegFreeUI.dll，构建需显式 HostBin。
- 安装位置：`C:/Program portable/3FUI/3FUI`，插件位于其 `Plugin` 下。覆盖前检查 FFmpegFreeUI/videoenhancer 均退出，备份再复制并校验哈希。
- 最新备份：`Artifacts/.refactor-tmp/backup-3fui-before-retry-enqueue-20261001-1241`，含原 DLL/EXE 及 installed-hashes.json。
- 最新部署 SHA256：DLL `6029B8BC853063DE0FB3571501A72D1A6D1952CACE5A574C84D92B86B2DA48DC`；EXE `FA2FC144722E56B9ED7F8F7B32B8B7B435B9B68677BBE5A77FA11FB81E0FB4E8`。
- 构建获取固定版 Aria2 Next 与 7-Zip，校验哈希并打包许可/源码；上游曾返回 502，重试成功。构建脚本用 pwsh，不增加用户运行依赖。
- 历史 GPU 矩阵存在真实 RTX 3060 测试证据；换机器或验证 GPU 专项时重查硬件、驱动与运行库。认证状态也需现查，Token 不进仓库。

## Verification And Commands

截至本轮已完成的证据（不是每次文档整理都重跑）：

- 原版/新版同 3.07 GB 包解压：245.66s / 65.41s；29,709 文件集合、大小、SHA256 全部相同；原生完整性检查通过。
- 下载取消集成 6/6、原 UI 取消探针 6/6、后端事务 6/6、单 EXE 离线工具释放与 ZIP 许可/源码检查通过。
- 最终下载队列测试 11/11、真实编译插件配假 CLI 界面 5 场景通过；取消→重试→重复点击仅重试一次。
- WiX 真实外层安装正常成功/无效目录失败/故障回滚、默认无临时日志、精确缓存/卸载/依赖状态清理门禁通过；内层目录/许可证/哈希/回滚门禁通过。
- 最终 solution build/publish 0 警告/0 错误，git diff --check 通过；本机部署哈希一致。用户本轮反馈“感觉没啥问题了”。

常用命令（在项目根执行，HostBin 按设备核实）：

```powershell
dotnet publish VideoEnhancer.slnx -c Release "-p:HostBin=<可用宿主程序集目录>"
dotnet run --project release/tests/DownloadQueue/DownloadQueue.vbproj -c Release
dotnet run --project release/tests/DownloadQueueUi/Probe.csproj -c Release -- (Get-Location).Path "<可用宿主程序集目录>"
pwsh -NoProfile -File release/test-installer-burn.ps1
git diff --check
```

文档整理仅验证归档字节/SHA256、UTF-8、链接和记录结构，不重跑程序测试。程序验收与发行详见 `release/发布流程.md`。

## Git Sync

- Repository: 当前根目录；branch main 跟踪 origin/main。
- HEAD：`ef7bee8 docs: close out 1.3.7 release status`；本次 git pull 返回 Already up to date。
- origin：`https://github.com/maxzrb/VideoEnhancer.git`；upstream：`https://github.com/user-Wing/VideoEnhancer.git`。
- 工作树非干净：本轮下载/缓存/解压/WiX源码、取消与原生工具、新测试和管理记录尚未提交；本次归档也是未提交改动。未执行提交、推送或发布。
- 用户确认后建议考虑提交；切换工具或设备前尤其要保存。不要覆盖现有修改或按旧归档执行破坏性 Git 操作。

## Session Log

以下前三条是当日重要工作的压缩摘要；逐条原文与此前记录见归档。

### 2026-10-01 11:32 - Codex（归档摘要）

- 教程缓存移入插件，下载异常进插件日志，原生解压与取消完成并部署。同包解压65.41s、29,709哈希一致，原始失败未复现。
- 保留 WiX，Cache=remove、默认日志关闭；真实成功/失败清除临时登记和缓存。旧59MB缓存与约18GB测试输出未清，后者受自动审批拒绝。

### 2026-10-01 12:14 - Codex（归档摘要）

- 按用户要求保留后端先行，模型三并发持续补位；取消和普通失败不终止批量，离线/认证停止补位，新增停止全部及分类计数。
- 队列8/8、界面4场景通过，构建并部署；版本未改、代码未提交。

### 2026-10-01 12:41 - Codex（归档摘要）

- 手动重试加入运行队列尾部，去重与空位立即唤醒；取消/失败计数转回待下载，未执行排队项在结束后恢复下载入口。
- 队列11/11、界面5场景通过，publish0/0；最新DLL/EXE部署哈希一致，候选安装器/ZIP同步重建。未发布、未提交。

### 2026-10-01 12:48 - Codex

- Request/orientation：用户反馈本轮“感觉没啥问题了”并要求压缩归档过期 STATUS/工作进度；记录当前交互验收通过。同工具续作，读取 AGENTS/INDEX/STATUS 和 HandShake；git pull 已最新，保留全部未提交代码。
- Changes：完整冻结旧 STATUS 和工作进度到 docs/archive/records-2026-10-01，当前状态改为有效快照、去重TODO、最新Git/环境及压缩日志；工作进度保留当日成果和当前摘要，INDEX增加按需历史导航。版本迭代记录内容保持不变，无程序或版本修改。
- Verification：归档字节及SHA256与整理前一致，UTF-8可解码、导航链接有效，必须状态章节与时间戳存在；git diff --check通过。未重跑程序测试。
- Next/Git：本轮和前序源码/记录均未提交，main同步origin/main，工作树非干净；建议考虑提交，切换工具/设备前保存。清理与长期专项待办保留，未升级为本轮执行任务。

### 2026-10-01 12:51 - Codex

- 用户追加要求汇报未完成事项；以压缩后的Active TODO归纳：提交/未来发布收尾、历史缓存与测试输出清理、GitHub大资产下载验证、历史专项验收及长期自动化/来源审计。当前下载交互已获用户可用反馈，不重复列为待修复。
- 文档终检：10个本地链接、9个必需状态章节、2份归档SHA256、UTF-8读取均通过；旧原文字节一致、版本记录未变，STATUS与工作进度分别缩减97.7%和98.4%。main同步origin/main，工作树未提交，建议考虑提交。

### 2026-10-01 13:06 - Codex

- 用户授权按发布流程发布1.3.8；同工具续作，读取AGENTS/INDEX/STATUS、HandShake与发布流程，git pull已最新。保留前序改动并纳入本次发行。
- 版本源、分类Release Notes和版本记录更新；build-modelscope-release生成最终EXE/安装器/ZIP/源码/stable，build/publish均0警告0错误。Backend审计29,709文件UNCHANGED、版本保持2026.09.30.1。
- 验证：Python33/33，队列11/11，编译插件UI5场景，发布门禁5/5，后端更新器6/6，历史补丁5/5；内层安装器、自更新隔离、最终WiX外层成功/失败/回滚与精确缓存/登记清理通过。历史测试首次误传多文件apphost缺DLL，改用最终单EXE后通过，非产品失败。ZIP原生工具/源码与EXE哈希、stable大小/hash一致。
- Git：main原HEAD ef7bee8，前序源码和记录尚未提交；下一步提交推送、标记v1.3.8并发布五项资产，同步ModelScope后回读。不重新打包或覆盖1.3.7。
