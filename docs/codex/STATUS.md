# Project Status

Last updated: 2026-10-01 17:54
Updated by: Codex

本文件是唯一 AI 操作状态来源。历史原文已冻结归档至 [记录归档](../archive/records-2026-10-01/README.md)，仅查历史时读取；归档中的版本、远端、环境和 TODO 不代表当前状态。

## Current Snapshot

- 2026-10-01 17:54：**1.3.9已按授权替换为8x修订版**。目标范围1–8，旧9–16x配置载入为8；源码与v1.3.9标签`b1cebcb`已推送，五资产/稳定清单/分类正文及两处ModelScope同步。11项实际下载哈希、故障回退通过，本机同版本替换成功且配置/用户能力清单/aria2保持。已安装旧1.3.9需手动更新；收尾记录提交推送后核对干净工作树。

- 2026-10-01 17:47：8x范围修订及旧配置兼容通过定向CLI/15项UI检查；最终build/publish0警告0错误、安装/自更新及ZIP/清单哈希通过，Backend审计仍UNCHANGED。冻结修订资产，开始按授权提交并替换1.3.9标签/GitHub与ModelScope资产。

- 2026-10-01 17:41：用户明确授权将目标输出倍率上限改8x并替换已发布1.3.9；CLI/插件/图片桥与旧配置载入同步调整，原生能力不变。原发行资产本地保留，按同版本覆盖流程重新构建、验证、更新标签及双源资产，正在实施。

- 2026-10-01 17:30：**1.3.9正式发布完成**：[GitHub v1.3.9](https://github.com/maxzrb/VideoEnhancer/releases/tag/v1.3.9)、ModelScope Releases及Models备用EXE已同步；源码/注释标签`0f38b64`已推送。五项资产双源及Models共11实际下载大小/SHA256通过，双源故障回退通过；本机正式升级1.3.9，配置/用户清单/aria2保持，未启动宿主。独立Backend继续2026.09.30.1。收尾文档提交推送后核对干净工作树。

- 2026-10-01 17:22：1.3.9本地构建和版本/清单/ZIP哈希通过，内层安装器与自更新隔离通过，Python33/33、倍率参数、UI12/悬停23、发布门禁5/后端更新6通过；独立Backend审计UNCHANGED。准备提交推送、标签和双源发布。

- 2026-10-01 17:17：用户明确授权正式发布1.3.9；已同步Git、确认凭据和版本未复用，版本源/发行说明/版本记录更新。构建、后端逐文件审计及最终版本门禁进行中，之后提交推送并发布双源；不重复完整GPU矩阵。

- 2026-10-01 17:01：用户截图反馈长路径/SHA-256仍裁切，已改为可选择复制的LakeUI只读多行文本框，字符自动换行并保留完整原文。两项长字段96高，实际落实固定说明/按钮行与小屏滚动；17项布局/字符覆盖检查通过，最终构建与本地部署哈希一致，配置保持。

- 2026-10-01 16:42：修复能力编辑弹窗底部裁切，路径/校验值、说明与按钮预留独立行，小屏可滚动。导入选择按钮按实际字体测量宽度。9项布局检查、编译/publish及diff检查通过，已部署本机，配置哈希不变。

- 2026-10-01 16:24：修复模型悬停提示窗释放后的复用与已显示状态阻止再次显示；提示关联当前菜单弹窗。介绍依据公开作者资料重写，纠正实拍/动漫定位、具体修复用途和过时限制，来源见[模型介绍依据](../model-introduction-sources.md)。23项提示/文案检查及12项倍率UI检查通过，已部署本机。

- 2026-10-01 15:54：用户选择/重新选择超分模型即恢复原生输出倍率(OutputScale=0)，两页同步；清单刷新与补帧选择保留目标。12项UI检查通过并部署本机。缩放算法保留Lanczos，本机mpv HQ放大ewa_lanczossharp、缩小catmull_rom，仅核对比较未移植。

- 2026-10-01 15:45：旧导入倍率已确认会传给RVE输出缩放覆盖；TRT用户改2x/3x可用源于真实推理后缩放，旧字段混合了两种倍率语义。

- 2026-10-01 15:30：修复切换模型后倍率提示滞后；菜单保存新模型ID后刷新，旧列表入口也同步刷新工作台/图片提示。8项UI检查通过，完整构建并再次部署本机，DLL/EXE哈希一致，未自动启动宿主。

- 2026-10-01 15:18：按用户要求已将模型架构/输出倍率改动部署到本机3FUI测试；正式自更新返回UPDATE_COMPLETE|1.3.8，DLL/EXE与完整构建产物哈希一致，现有插件配置哈希未变，未自动启动宿主。

- 2026-10-01 15:10：模型架构家族统一与原生/输出倍率分离已完成；98项逐项审计，修正42项内置架构/输入约束/多倍率声明。详见[审计报告](../model-capability-audit.md)与[逐项证据](../model-capability-audit.json)。代码尚未提交；随后按用户要求已部署本机测试，版本号保持1.3.8，不发布远端。
- 新增 `-output-scale 1–8` 与工作台/图片页选择，默认原生；固定权重先原生推理再Lanczos缩放，FlashVSR可直接2x/4x。TensorRT缓存按推理倍率复用。旧用户路径、ID与能力记录不自动改写；重新检测先展示差异、载入修正窗口后由用户保存。

- 上一正式版本 **1.3.8 / 2026-10-01**：GitHub [v1.3.8](https://github.com/maxzrb/VideoEnhancer/releases/tag/v1.3.8)、ModelScope Releases及Models备用EXE已发布，标签指向源码提交 `2895ce8`；未覆盖旧版本。
- 本日下载/解压/缓存/WiX改动已纳入1.3.8；本地构建与门禁、11项双源文件实际下载哈希、双源故障回退均通过。宿主退出后经正式自更新入口部署本机，备份与EXE/DLL哈希已核对；未自动启动宿主，重启后的真实窗口观察仍由用户完成。
- Python Backend沿用 **2026.09.30.1**：29,709文件审计UNCHANGED，远端channel仍保留2条历史补丁，不发布空更新。RTX runtime、模型、配置与独立组件未更换。
- 教程缓存位于 `Plugin/videoenhancer/cache/TutorialImages`，迁移已知旧图片缓存，保留冲突/无关文件。下载完整异常进 `logs/downloads.log`，文件部署失败进 `logs/installer.log`；错误提示样式与五秒显示时长保持。
- 7z 使用便携 7-Zip 26.03 x64 多线程解压，SharpCompress 先做归档路径/链接预检，原生 CRC 保留；单运行 EXE 内含工具、许可证和对应源码。界面仅显示解压百分比，不显示文件名；取消轮询只在任务有标记时启用。
- 下载全部先单独完成后端安装，再按列表顺序最多三个模型任务并行、持续补位。取消或普通失败不阻断队列；离线/认证问题停止补位并等当前任务结束。手动重试加入队尾，重复点击去重，有空位立即开始，按文件最新状态统计；不自动重试。
- 下载全部运行时可点击停止全部，停止补位并取消当前可取消任务；后端替换事务不强杀，安全结束后不启动模型队列。分类下载采用相同队列规则。Windows Job 负责宿主退出后的下载子进程清理。
- 保留现有 WiX Burn 6.0.2 窗口；链仅含 Permanent EXE，Cache=remove，默认 Burn 日志关闭，/log 诊断保留。实测正常成功/正常失败后无本次载荷缓存、Bundle 缓存、临时卸载和依赖登记。不是 MSI；旧缓存及异常强杀/断电、Windows 自身记录不保证清零。

## Active TODO

- [ ] 本机重启3FUI测试能力修正弹窗/导入按钮宽度和模型菜单重复打开/切换分组后的悬停介绍、模型选择恢复原生倍率及导入重新检测；本轮已部署且哈希核验通过，未自动启动宿主。
- [ ] 维护清理：本机旧版三份载荷缓存约 59 MB；项目三份完整解压验证输出约 18 GB。后者曾两次被自动审批以 blocked by policy 拒绝，未绕过。确认归属与路径后再单独处理，勿清系统共享缓存。
- [ ] 历史发布验证补充：1.3.7 的 GitHub 大资产此前 CDN 下载超时，仅 API digest/大小核对；ModelScope 实际下载哈希通过。网络恢复后可补 GitHub 大资产实际下载校验。
- [ ] 长期可选工作：发行自动化与上游选择性同步清单；模型镜像逐文件来源/授权审计。项目自身 MIT 已落实，不沿用旧的“项目许可证未定”说法。
- [ ] 遗留专项验收记录：高 DPI/模型菜单/预览压力/四宫格完整交互、真实 TensorRT 缓存及组合视频专项在旧记录中未全部补最终验收；后续相关修改时定向复验。本次用户反馈仅关闭本轮下载交互验收，不代表全部历史专项完成。

旧预览目录、已发布 1.1.0、已合并 PR 和已被新后端取代的验证任务已退出当前 TODO，原始上下文保留在归档；不得按旧记录恢复到预览主线或旧后端。

## Recently Completed

- 2026-09-30：正式发布 1.3.7，教程图片滚动缓存优化；GitHub/ModelScope 发布记录已提交并推送。
- 2026-10-01：便携教程缓存、原生解压、取消和日志、WiX 安装残留收敛、解压百分比显示、持续补位与手动重试入队完成；本机已更新，用户反馈无明显问题。
- 2026-10-01：当前操作记录压缩、完整原文归档、导航更新；归档入Git后的字节/SHA256再次核对一致。
- 2026-10-01：正式发布1.3.8及1.3.9，均完成双源回读、故障回退和本机正式自更新；1.3.9包含模型能力/倍率/介绍与导入界面专项。

## Decisions

- 当前根目录 main 是唯一开发主线；origin 为独立维护仓库，upstream 仅供选择性移植。不沿用旧 fork/origin 名称或 preview 工作目录。
- 保留 LakeUI 界面和当前 WiX 安装器；用户明确拒绝已有便携自解包交互窗口。
- 后端体积敏感，先单独安装；模型数量敏感，使用三并发滑动窗口。单项取消与停止全部分开，失败/取消不自动重试，手动重试可加入运行队列。
- 错误完整信息写插件日志；不改五秒错误提示，不展示解压文件名。减法针对运行期查询/轮询/输出开销，不宣称安装包更小。
- 保留后端增量路由、完整修复确认和事务回滚；半成品不标为安装成功，取消保留可重试缓存/断点。
- 项目 MIT 与第三方各自许可证并存；Aria2 Next Release 源码资产供许可证履行，普通用户不用另外安装该源码包。
- 跨后端/模型能力、转场阈值和分块语义以当前源码为准；历史被推翻的决定仅查归档，不从旧快照恢复默认值。

## Risks And Blockers

- 1.3.8双源发布、实际下载和故障回退已验证；原始IO/end失败根因仍未确证，后续复发读取插件日志。
- 用户最初的 99% 五分钟后 IO/end 类失败未复现；原实现同包最终成功，不能把根因写成已确证。再次发生时读取插件日志。
- 原生工具/源码内嵌增加运行 EXE 约 2 MB；优化结果是解压耗时与运行开销降低，不是文件体积下降。
- 历史“无 NVIDIA/缺宿主程序集/只隐藏控制面板”等过时阻塞已移出当前状态；当前设备曾完成真实 GPU 验证，本轮也已有可用构建引用。换设备后仍须重查。
- 旧清理拒绝和 GitHub CDN 验证限制见 Active TODO；不将历史注册残留推测当实测事实。

## Environment Notes

以下仅为本机已验证路径，不能当作跨设备配置。

- 工作区：`C:/Codex Program/3fui plugin`；Windows / PowerShell，.NET 10，文件读写 UTF-8。
- 本轮构建 HostBin：`C:/Users/maxzr/AppData/Local/Temp/3fui-core-compat-host`，LakeUI 5.9。实际宿主发布根不含可用 FFmpegFreeUI.dll，构建需显式 HostBin。
- 安装位置：`C:/Program portable/3FUI/3FUI`，插件位于其 `Plugin` 下。覆盖前检查 FFmpegFreeUI/videoenhancer 均退出，备份再复制并校验哈希。
- 最新发行部署备份：`C:/Codex Program/3fui plugin/Artifacts/.refactor-tmp/backup-3fui-before-1.3.9-8x-20261001-175231`，含替换前EXE/DLL与插件配置；证据Artifacts/.refactor-tmp/release-1.3.9-8x/local-deployment.json。
- 当前安装正式1.3.9（8x修订）：EXE SHA256 `98519EDFD9F25D25CFE151F38BC6AAD62763905025C0D9E5BDB3AD08CAED84DB`，DLL `1040B389AF9877EF3C6AF3E152B215F436B809B4E6A5DC5363147969685E2998`；升级前文件完整备份。CLI实际位于Plugin/videoenhancer/videoenhancer.exe。
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
- 最新发行修订提交：`b1cebcb fix: cap output scale at 8x for 1.3.9`，main及注释标签v1.3.9已同步origin；用户明确同版本替换授权，tag以精确lease更新，main未强推。首次发行0f38b64仍保留历史，收尾文档另作提交。
- origin：`https://github.com/maxzrb/VideoEnhancer.git`；upstream：`https://github.com/user-Wing/VideoEnhancer.git`。
- 所有前序源码、测试和归档已提交；发行产物、上传缓存与验证夹具留在忽略目录，不纳入Git。收尾提交后工作树应干净，切换设备前确认远端同步。

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

### 2026-10-01 13:15 - Codex：1.3.8发布收尾

- Release：https://github.com/maxzrb/VideoEnhancer/releases/tag/v1.3.8；源码2895ce8已推送main，注释标签v1.3.8同指向，正式稳定版、五项资产、逐行分类正文与本地一致。
- Mirrors：modelscope upload AerithDream/VideoEnhancer-Releases release/dist/modelscope --repo_type dataset（6文件提交、0失败、0删除）；Models同路径备用EXE使用--no-cache上传，未动模型/后端资产。
- 回读：GitHub五文件、ModelScope五文件及Models备用EXE共11项HTTP200/大小/实际下载SHA256全部一致；两份stable清单完全一致，README优先级正确。模型列表96项无重复，Plugin/videoenhancer.exe仅1项、大小正确。Backend通道latestVersion2026.09.30.1、两条补丁保持。
- 环境排障：首次Python requests GitHub证书校验因本机CA集合不足失败，改用已安装truststore读取Windows可信证书后全部通过，未关闭TLS校验；重复gh下载已停止。
- 故障回退：最终编译插件实测GitHub不存在仓库→ModelScope清单1.3.8；ModelScope无效数据集→GitHub包，最终SHA256正确。首个夹具误用无效GitHub配置格式同时影响包URL，改为真实404仓库后验证通过；产品未改。
- 本机：确认FFmpegFreeUI/videoenhancer退出，备份后用最终EXE --apply-update --wait-pid 0执行正式升级，UPDATE_COMPLETE|1.3.8；EXE/DLL哈希与发行产物一致。未自动启动3FUI，未重复GPU处理测试；当前设备历史RTX3060/TensorRT验证见归档。
- 证据：Artifacts/.refactor-tmp/release-1.3.8中的asset-hashes、remote-verification、remote-backend-channel、remote-models、local-deployment与故障回退夹具；本机备份C:/Codex Program/3fui plugin/Artifacts/.refactor-tmp/backup-3fui-before-release-1.3.8-20261001-131000。归档入Git字节/hash一致，冻结目录属性保留原换行/历史空白。
- aria2-next-2.5.6-source.tar.gz: 2493064 bytes / SHA256 `0a1e324cc8ddae583e3d3b18411594ee24f24d210ee7216cf11872e3454ccb72`。
- VideoEnhancer-1.3.8-manual-install.zip: 18601773 bytes / SHA256 `f38631bbe20d6b29afc7480003d3ddb60567630feebd1f7c7c1deed30bdc3128`。
- VideoEnhancer-1.3.8-win-x64.exe: 17303147 bytes / SHA256 `71d3f724fdae745a84d1b97afc5933f432ebe04561b7d04cf6845191bcad7e64`。
- VideoEnhancerInstaller-1.3.8-win-x64.exe: 16079246 bytes / SHA256 `bcbb86c4f5492ca8a55f94a288313e320677c569e814ff7756b49c279152342d`。
- stable.json: 1175 bytes / SHA256 `b2cfb285ff406f3fb6c9d6f367c864f68936222f4a0c71ae9c82ce24ec786d13`。
- Records/Git：STATUS、中文工作进度和版本记录更新；发行发布任务已完成，旧缓存/测试输出清理及历史专项/长期审计仍为独立待办。最后提交推送收尾文档并核对干净工作树。

### 2026-10-01 14:30 - Codex：模型能力专项启动

- 同工具续作，已读取AGENTS/INDEX/STATUS及HandShake；git pull --ff-only已最新，起始main工作树干净。
- 用户确认按真实架构家族归组，覆盖全部超分/修复模型，原生倍率与1–16x目标输出倍率分开；补帧不纳入。
- 本机Torch2.9.0+cu130/CUDA可用，ONNX1.22.0；本机模型只读，审计输出在Artifacts/model-audit。逐项98条读取完成，发现Compact/ESRGAN误归类、SPAN命名与内容不符、RRDB输入倍数不足，以及检测器架构字段为字符串时预检失败。
- 代码实施与定向验证进行中；未发布、未部署、未更改现有用户模型记录。

### 2026-10-01 15:10 - Codex：模型架构与倍率专项收尾

- Changes：统一architectureGroup；修正Compact/ESRGAN/RRDB/HAT/SPANPlus等实际归类与RRDB/DITN/CRAFT输入约束；模型ID、安装路径、文件名和权重保持不变。模型清单仍schemaVersion1，新增字段向后兼容。新增原生/目标输出倍率接口、同步界面、导入重新检测；1x修复模型不再被工作台排除。
- Detection：PTH由真实权重与描述器识别，处理字符串架构和AnimeSR专用类；ONNX架构来自图，22个动态图CPU探测输出倍率，不按文件名猜倍率；NCNN已审计图按param内容SHA256匹配，执行传真实param/bin基名，导入改目录名仍可用。检测缓存按文件路径/大小/修改时间，位于便携cache，旧用户清单不自动写回。
- Scaling：新增-output-scale1–16，默认原生；视频在最终编码滤镜末尾Lanczos缩放、图片保存前缩放；显式目标替代预设-s尺寸。旧-scale只接受真实推理倍率；FlashVSR记录支持2x/4x。图片后端接收原生倍率，不再自行猜已知模型；固定TensorRT权重缓存不因目标倍率新增Engine。RTX与分段继续各自输出规格。
- Files：cli能力清单/分组/检测缓存/NCNN图签名/倍率与参数/嵌入脚本；插件模型DTO/菜单/提示/导入管理/图片与工作台/队列配置；新增docs/model-capability-audit.md和.json、审计与定向运行脚本、C#倍率与UI探针；STATUS及中文工作进度。项目版本未改，版本迭代记录保持不变。
- Verification：98项结构/倍率核对无未识别或预检错误，42项清单修正。37个不同实际运行场景通过（分三轮25/12/8，基础4项复核去重）：CUDA/TRT图片与视频原生/2x/3x/4x、原生4xEngine复用、重命名PTH/ONNX/NCNN、保留旧人工记录、1x修复与TRT、BasicVSR++、FlashVSR直接2x、两种组合顺序与NCNN→CUDA跨后端、现有crop和-s最终输出尺寸。全部模型没有逐个GPU推理；结构审计和代表推理证据分开。
- Commands：python -m unittest discover -s cli/tests -p test_*.py（33/33；异常传播测试的预期traceback不是产品失败）；dotnet run cli/tests/ModelMetadata（倍率范围/滤镜/参数/分组）；dotnet run cli/tests/ModelMetadataUi（6项界面状态，不显示窗口）；solution Release build与最终CLI自包含裁剪publish均0警告0错误；git diff --check通过。最终代码变化只定向重验，不重跑下载/安装器/完整GPU矩阵。
- Environment/evidence：实际Torch2.9.0+cu130/CUDA可用、ONNX1.22.0；HostBin仍用本机已记录temp兼容宿主。隔离测试在Artifacts/model-audit，后端为实际复制，python解释器及各只读模型目录为本机junction，User和TensorRT-Cache均在隔离目录；本机路径仅作环境记录。初次测试FFmpeg仅复制EXE缺DLL，补齐隔离目录DLL后通过；VB变量Scale与继承成员冲突、ChrW命名以及裁剪JSON反射在实施中修正，最终编译无警告。
- Deployment/Git：未发布、未部署、未自动启动宿主；安装EXE SHA256仍为71D3F724FDAE745A84D1B97AFC5933F432EBE04561B7D04CF6845191BCAD7E64。main同步origin/main，但本轮代码/报告/记录未提交，工作树非干净；切换工具/设备前建议考虑git提交。原有长期TODO不变，专项实施已完成，下一步按用户指令决定提交或部署。
### 2026-10-01 15:18 - Codex：模型专项部署本机测试

- Authorization：用户明确要求“部署到我本地3fui测试”，覆盖先前不部署的默认限制；不发布远端、不递增版本、不自动启动宿主。
- Startup/Git：同工具续作，沿用已读AGENTS/INDEX/STATUS及HandShake；git pull --ff-only已最新，main跟踪origin/main，本轮实施改动保留未提交。
- Build：dotnet publish VideoEnhancer.slnx -c Release，HostBin沿用已核实兼容宿主目录；完整solution发布成功，更新EXE嵌入本轮插件DLL。附带本地打包产物没有上传或执行安装器，不重复下载/GPU压力测试。
- Deployment：确认FFmpegFreeUI/videoenhancer均退出；备份旧DLL、EXE与插件配置到`C:/Codex Program/3fui plugin/Artifacts/.refactor-tmp/backup-3fui-before-model-architecture-20261001-151724`；用Artifacts/videoenhancer.exe --apply-update --update-package <同一构建EXE> --update-target <本机Plugin目录> --wait-pid 0，返回UPDATE_COMPLETE|1.3.8。
- Verification：安装EXE SHA256 `F98B5B79BD20521E4E0A493FABABBBCB05EE9683DBEC78649E9E9AAE409C0221`，DLL `5047F0DC711329BCFD00CCF218B99E910269FC989A8AEDB89880CDC30858268A`，均与构建产物一致；现有videoenhancer.plugin.json哈希与备份一致，未手动改写用户能力记录或移动模型文件。部署证据Artifacts/model-audit/local-deployment.json；git diff --check通过（仅autocrlf提示）。
- Closeout：STATUS与中文工作进度更新，版本迭代记录不变。工作树非干净，代码/审计/记录仍未提交，建议本机测试通过后考虑git提交；下一步由用户启动3FUI检查模型分组、倍率菜单和重新检测交互。
### 2026-10-01 15:30 - Codex：模型切换自动更新倍率提示

- 同工具续作；读取AGENTS/INDEX/STATUS及HandShake，git pull --ff-only已最新，main跟踪origin/main；保留前序未提交修改。
- 原因：SetCatalogSelection先SyncOutputScaleControls后保存_config.Model，提示使用旧模型；旧OnModelSelected没有刷新提示。
- 修复：菜单先保存选择再刷新倍率控件；旧列表选择保存后同样刷新。工作台与图片页同步，未添加额外弹窗。变更文件为PluginPanel.UpscalePage.vb和cli/tests/ModelMetadataUi/Program.cs。
- 验证：插件Release build 0警告0错误；UI探针8项通过，新增实际模型切换2x→4x断言，立即展示“原生 4x；原生推理后缩放至 2x”，两个页面一致；完整solution publish成功，git diff --check通过。不重复GPU、下载或安装器测试。
- 部署：沿用用户本地测试授权，检查FFmpegFreeUI/videoenhancer退出，备份`C:/Codex Program/3fui plugin/Artifacts/.refactor-tmp/backup-3fui-before-scale-hint-20261001-153010`后正式--apply-update --wait-pid 0成功；安装EXE `3D7CCA4A37D0C76ED40B3CC32E021188A9C72F3987E385F4E2B3677AD5637E8B`、DLL `E96E772DAC75C293BB890E7453E66F3EADEE931011081F060AAE6365F791BC83`与构建产物一致，现有配置哈希未变，未启动宿主/发布远端/更改版本。
- 收尾：STATUS与中文工作进度更新；工作树非干净，本轮和前序修改尚未提交，建议测试通过后考虑git提交。下一步用户启动3FUI切换不同原生倍率模型检查即时提示。

### 2026-10-01 15:45 - Codex：确认旧导入倍率与TRT缩放机制

- 用户补充：同一内置AnimeVideo v3权重自行导入、修改记录倍率，在TensorRT使用2x/3x。只读核对HEAD旧Program/ModelCapabilityCatalog及本机实际RVE后端。
- 旧清单直接使用user.Scale；CLI将倍率传给--override_upscale_scale。RenderVideo由模型/Engine获取真实modelScale，在超分后resize_image_bytes调整目标尺寸；缩小采用INTER_AREA，放大采用INTER_LANCZOS4。因此旧导入倍率混用了原生与目标输出语义，用户反馈不能据此证明固定4x权重原生2x/3x。
- 本轮新增输出倍率是显式分离既有能力，而不是首次让TRT可以输出2x/3x；新版最终Lanczos与旧RVE缩小INTER_AREA不是完全相同的算法。未取得该用户运行日志，不能确认其当次Engine具体缓存状态。
- 未改产品代码、未再次部署/测试，main前序改动仍未提交；已更新中文进度，建议本机验收后考虑git提交。

### 2026-10-01 15:54 - Codex：模型选择恢复原生倍率与mpv算法核对

- 用户要求模型重新选择自动采用其原生倍率，并询问Lanczos与本机mpv变体。沿用AGENTS/INDEX/STATUS及HandShake，同工具续作，git pull --ff-only已最新；main前序改动保留未提交。
- Changes：SetCatalogSelection的保存超分选择分支及旧OnModelSelected设置OutputScale=0，随后保存并同步工作台/图片页。即使重新选择同一模型也恢复原生；清单刷新(saveConfig=False)与补帧选择不重置。
- Verification：插件Release build 0警告0错误；ModelMetadataUi 12项通过，涵盖恢复原生/两页同步/清单刷新保留/补帧不影响/显式修改后提示；完整solution publish成功，git diff --check通过；未重复GPU、下载或安装器测试。
- Algorithms：只读本机MPV Vanta Edition与mpv-full-private-v1.3.0的portable_config/mpv.conf和profiles.conf，两套启用HQ，scale=ewa_lanczossharp、dscale=catmull_rom、scale-antiring=0.5、sigmoid-upscaling=yes、correct-downscaling=yes、linear-downscaling=no。核对mpv官方manual stable：EWA为Jinc滤波，sharp有锐化；升降采样可分别设置，不是所有缩放均用同一Lanczos变体。建议当前输出仍保留Lanczos；未做画质A/B，不能宣称EWA必然更好。未修改本机mpv配置或产品缩放算法。
- Deployment：检查FFmpegFreeUI/videoenhancer退出，备份`C:/Codex Program/3fui plugin/Artifacts/.refactor-tmp/backup-3fui-before-native-reset-20261001-155334`后正式自更新UPDATE_COMPLETE|1.3.8。EXE `9A824BE308BFE4ED88EE0176FE2AFFDFD631F05C40086416728FBBC8A7A5709B`、DLL `955D9EFBD7A3EB82CBE5F1FD12ED9748220A2A9D5D4A9751021CD5D3F75F4AA9`与构建一致，插件配置哈希保持，证据Artifacts/model-audit/native-reset-deployment.json；未启动宿主或发布远端，版本不变。
- 收尾：STATUS与中文进度更新，代码/审计/记录尚未提交，工作树非干净；建议用户本机测试后考虑git提交。画质算法对比仅为建议，未新增为必须实施任务。

### 2026-10-01 16:24 - Codex：模型菜单悬停与公开介绍修正

- 启动：同工具续作，沿用已读AGENTS/HandShake，复读INDEX/STATUS；git pull --ff-only已最新，main跟踪origin/main，前序实施未提交。用户重复请求及“继续”均作为本任务续作。
- Lifecycle：核对项目暂存的LakeUI源码，FloatingToolTipForm会因关联控件/窗体状态调用Close并释放；原控制器保留readonly窗体且已显示项直接短路。改为延迟创建、关联当前菜单popup、失效/换分组时重建；只有实际可见且未释放才认为已显示；菜单关闭仍释放计时器/窗体/字体。不修改LakeUI或用户mpv。
- Descriptions：ModelDescriptionProvider重写并精简名称/倍率/来源重复。公开信息优先说明训练素材、处理问题与版本差异；纠正OpenProteus实拍、Nomos8k照片、AniSD AC/DC/DB/PS用途、AniScale2 DITN限制与Refiner顺序；解释GIMM RAFT/FlowFormer/LPIPS；删除FlashVSR不能与补帧组合的旧说法和无依据的架构画风/版本排名。RealHatGAN/fix导出、BHI/Sudo/ModernSpanimation训练差异仍待复核，不夸大支持。
- Sources：新增docs/model-introduction-sources.md，记录21组作者仓库/模型卡/论文与限制；OpenProteus发布正文用GitHub API补读。网络搜索曾有错误仓库路径/页面不可读，最终使用正确官方地址；未把未取得内容的链接当证据。98条实际文案导出Artifacts/model-audit/model-introductions.json，长度均不超过180字符，结构能力以先前权重审计为准。
- Files：PluginPanel.vb、PluginPanel.UpscalePage.vb、ModelDescriptionProvider.vb；ModelMetadataUi/Program.cs及新增Program.Tooltips.cs；公开依据文档、STATUS和中文进度。注释中文、UTF-8及源文件CRLF保持。
- Verification：Release插件build 0警告0错误；完整solution publish成功。--tooltips在不切换到前台的独立Windows测试桌面运行，实际菜单三次打开、提示自关释放后重建、切换popup、关闭释放共15项；7项关键文案与98条长度检查共8项，总23项通过。既有倍率UI探针12项通过；git diff --check通过，仅autocrlf提示。未启动本机宿主、移动用户鼠标或重复GPU/下载/安装器测试。初次STA已占窗口资源无法SetThreadDesktop，改新线程先关联桌面后运行，最终无警告。
- Deployment：沿用本地测试授权，确认FFmpegFreeUI/videoenhancer退出，备份`C:/Codex Program/3fui plugin/Artifacts/.refactor-tmp/backup-3fui-before-model-tooltips-20261001-162313`后正式自更新返回UPDATE_COMPLETE|1.3.8；EXE `C85DC315D40FF3D0312F46D326ADD29644516A47CF367F00F4ED9BE67EF1103F`、DLL `12E00DD13C5312865B99CCE97848FAD7517A560CAFEF02D5C5175A575E200A85`与完整构建一致，现有插件配置哈希未变。证据Artifacts/model-audit/tooltips-deployment.json；不自动启动宿主、不发布远端、版本不变。
- 收尾：记录已更新，工作树非干净，前序和本轮代码/文档尚未提交；建议用户在实际窗口检查悬停交互后考虑git提交。无新增执行阻塞，实际宿主观察仍由用户完成。

### 2026-10-01 16:42 - Codex：导入能力弹窗与按钮布局

- 启动：同工具续作，读取AGENTS/INDEX/STATUS及HandShake；git pull --ff-only已最新，main同步origin/main，保留前序未提交改动。
- 变更：PluginPanel.ModelImportPage.vb能力编辑窗口加宽、标签列180，路径/校验值64高，说明56与操作60各占独立行；内容高度与标题padding共同决定窗口高度，小屏用LakeUI垂直滚动。选择模型/文件夹按钮最小210，按实际字体文字宽度+48测量并随FontChanged更新。能力值与保存逻辑不变。
- 验证：Release插件build和完整solution publish均0警告0错误，git diff --check通过。临时Artifacts/model-audit/import-layout-probe验证实际模态弹窗9项通过：两按钮12/18pt留白4项、说明和保存/取消完整3项、六后端及双行路径2项。96DPI客户区820x660，内容818x624；实际高DPI仍由用户观察。独立桌面创建完整插件页停滞，仅终止自有测试进程后改STA普通桌面短暂弹窗验证；路径高度初始断言未计margin，改按真实双行字高。DrawToBitmap是黑图，不作为视觉证据。未增加正式测试或重复GPU/下载/安装器测试。
- 部署：确认宿主/CLI退出，备份旧DLL/配置到`C:/Codex Program/3fui plugin/Artifacts/.refactor-tmp/backup-3fui-before-import-layout-20261001-164012`；CLI备份和首次哈希路径误写为Plugin/videoenhancer.exe，非终止错误后正式自更新仍返回UPDATE_COMPLETE|1.3.8。按真实子目录重新验证EXE/DLL与构建一致，配置与更新前备份哈希一致。旧CLI未留存本轮备份，记录限制；没有重复更新。证据Artifacts/model-audit/import-layout-deployment.json；未自动启动3FUI、不发布远端、版本不变。
- 收尾：STATUS与中文进度更新，main同步origin/main但工作树非干净，前序/本轮均未提交；建议用户本机验证后考虑git提交。

### 2026-10-01 17:01 - Codex：长路径及SHA-256完整显示

- 同工具续作，复核INDEX/STATUS，沿用AGENTS及HandShake；git pull --ff-only已最新，main同步origin/main，前序未提交改动保留。用户截图确认上次行高断言不能证明完整显示。
- 变更：ModelImportPage两个只读长字段由LakeTextLabel换为LakeUI ModernTextBox，ReadOnly/MultiLine/WordWrap启用，完整文本可选择复制，多行滚动支持异常长值；字段各96高。固定说明56/按钮60行与LakeUI垂直滚动容器实际加入，窗口高度显式加标题/边框而不依赖Attach之后尚未生效的Padding。
- 纠正上次记录：先前机械替换的多行片段因CRLF匹配未落实，实际仅窗口宽高/标签列加大；16:42记录关于固定行/滚动的描述超出当时源码。本轮按源码片段明确替换并复读确认，不改写历史日志。
- 验证：插件Release build及最终solution publish均0警告0错误，git diff --check通过。临时UI探针17项通过：原9项布局加两个长字段各4项（只读换行、实际宽度与300px下每个字符均进入视觉行、完整选择）。测试使用真实弹窗/实际LakeUI视觉行，不以区域高度或黑色DrawToBitmap推断文本完整。96DPI窗口820x724、内容818x688。首次试改未命中原行配置，代码复读后落实；VB循环变量Height与控件属性冲突改rowHeight，窄宽度测试先SuspendLayout避免父布局重设，最终通过。未新增正式测试/重跑GPU或下载测试。
- 部署：宿主/CLI退出后完整备份到`C:/Codex Program/3fui plugin/Artifacts/.refactor-tmp/backup-3fui-before-import-details-20261001-170035`，正式自更新UPDATE_COMPLETE|1.3.8；EXE/DLL均与最终构建一致，插件配置与备份哈希一致，证据Artifacts/model-audit/import-details-deployment.json。未发布远端、未更改版本、不自动启动3FUI。
- 收尾：STATUS及中文进度更新；main同步远端但工作树非干净，代码与记录未提交。用户实际窗口查看仍待反馈，建议确认后考虑Git提交。

### 2026-10-01 17:17 - Codex：1.3.9发行启动

- 同工具续作，读取AGENTS/INDEX/STATUS及发布流程，沿用HandShake；git pull --ff-only已最新，main全部未提交变化属于本轮模型专项。
- 用户“发布1.3.9release”授权提交/推送/标签/双源正式发布与流程内本机升级，覆盖先前不发布限制。两项目Version设1.3.9、逐行分类Notes及版本历史保留完成；后端独立包须脚本审计，Python检测/图片桥更新为CLI嵌入资源。
- 前序模型结构/代表GPU验证沿用，运行最终发行所需门禁，不重复完整下载/安装器压力或完整GPU矩阵。下一步本地构建/验证→提交推送/发布→远端与本机升级校验。

### 2026-10-01 17:22 - Codex：1.3.9本地门禁

- build-modelscope-release生成最终五项资产；build/publish0警告0错误，CLI1.3.9与两项目版本一致，stable大小/hash、ZIP内DLL/EXE与构建一致，GPL源码hash正确。内层安装器、自更新成功及回滚通过。
- Python33/33与两项修改脚本py_compile、ModelMetadata倍率/分组参数、ModelMetadataUi12及悬停23、release门禁5与backend更新6通过；预期错误日志属于异常/回滚夹具。前序98模型结构与37种GPU代表场景、17导入布局沿用，本轮不重复GPU矩阵。
- Backend原已发布2026.09.30.1的29709文件逐项SHA256审计UNCHANGED，本轮桥脚本由CLI嵌入同步，独立完整包/channel不发布空更新。最终资产哈希保存在Artifacts/.refactor-tmp/release-1.3.9/asset-hashes.json。
- 接下来完成最终WiX正常/失败缓存门禁，提交本轮源码与版本记录并推送origin main/标签；GitHub发布后上传相同dist到ModelScope，禁止重新打包造成哈希变化。

### 2026-10-01 17:30 - Codex：1.3.9双源发行收尾

- Release：https://github.com/maxzrb/VideoEnhancer/releases/tag/v1.3.9；提交0f38b64已推送main，注释标签v1.3.9同指向。正式稳定版/latest，五项资产且分类正文与本地Notes完全一致。
- ModelScope：Releases目录39项中跳过33项，提交6项，0失败/0删除；Models备用Plugin/videoenhancer.exe用--no-cache同步，复用同hash对象，未动权重/Backend/channel。两份stable JSON完全相同，README下载优先级正确。
- 回读：GitHub五文件、ModelScope五文件及Models备用EXE共11项HTTP200、实际大小与SHA256均与冻结dist一致。模型远端96项、无重复/PotPlayer，备用EXE唯一且大小17312224。Backend线上latestVersion2026.09.30.1，两条历史补丁保持；独立包29709文件审计UNCHANGED，本轮辅助脚本由CLI嵌入更新，不发布空后端版本。
- 最终门禁：build/publish均0警告0错误，Python33/33和修改脚本py_compile、ModelMetadata倍率/分组、ModelMetadataUi12及悬停23、发布门禁5、后端更新6通过。build脚本内层安装/自更新成功和回滚通过；最终WiX外层成功/无效目录/回滚及精确缓存/注册清理通过；ZIP内EXE/DLL/aria2/许可与stable校验通过。前序98模型结构、37种不同GPU代表场景、17导入布局证据继续有效，未重跑完整GPU矩阵；本机有NVIDIA并已完成本轮实际TRT图片/视频代表推理，未宣称所有模型逐个GPU验收。
- 双源回退：用最终编译插件将GitHub检查配置指向真实404仓库，回退ModelScope读到1.3.9；将ModelScope下载数据集设无效，实际回退GitHub下载1.3.9且SHA256正确。测试只在项目隔离夹具，不改变真实用户配置。
- 本机升级：确认FFmpegFreeUI/videoenhancer退出，完整备份到`C:/Codex Program/3fui plugin/Artifacts/.refactor-tmp/backup-3fui-before-release-1.3.9-20261001-172635`，执行冻结发行EXE --apply-update --wait-pid 0返回UPDATE_COMPLETE|1.3.9；安装CLI --version=1.3.9，EXE/DLL哈希匹配，videoenhancer.plugin.json、models/User/model-catalog.json和aria2-next.exe哈希保持。未自动启动宿主，实际高DPI/窗口目视继续由用户反馈。
- 证据：Artifacts/.refactor-tmp/release-1.3.9中的asset-hashes、github-release、remote-verification、remote-models、remote-backend-channel、local-deployment、backend-audit与updater-probe；不纳入Git。
- 来源/风险：模型介绍公开依据和待复核项已写docs/model-introduction-sources.md；结构能力依据权重审计，本轮不新增/重传模型权重；模型再分发来源/授权逐文件复核保留长期TODO。原历史清理/其他专项待办不扩大为本轮任务。
- 收尾：STATUS、中文工作进度及版本迭代记录更新，发行任务已完成；main代码与标签同步origin，最后提交推送收尾文档并核对干净工作树，切换工具/设备前无需遗留未提交源码。

- 1.3.9发行资产：
  - aria2-next-2.5.6-source.tar.gz: 2493064 bytes / SHA256 `0a1e324cc8ddae583e3d3b18411594ee24f24d210ee7216cf11872e3454ccb72`。
  - VideoEnhancer-1.3.9-manual-install.zip: 18611136 bytes / SHA256 `869421a26080208a5f1e41892e2f55a7fec79ee6127d95f9a136888c39af845c`。
  - VideoEnhancer-1.3.9-win-x64.exe: 17312224 bytes / SHA256 `e5acdf9c681224d04421811f4b601066cc5d454adff172ed1880fc7eeacd0f94`。
  - VideoEnhancerInstaller-1.3.9-win-x64.exe: 16089960 bytes / SHA256 `121a0bb8eee61254d0e3da2d00ed779e64986648f45fdcc167be5f28ae094072`。
  - stable.json: 1592 bytes / SHA256 `61049243ef96f2b549837cd913ad3e2f339ee9a3beabe9f814ce61ad021b6e8e`。

### 2026-10-01 17:41 - Codex：1.3.9同版本8x修订启动

- 同工具续作，复核AGENTS/INDEX/STATUS，沿用HandShake与发布流程；git pull --ff-only已最新，起始main干净。用户明确授权替换1.3.9，同步GitHub资产、稳定清单、ModelScope Releases/Models及标签，保留旧版资产证据在Artifacts/.refactor-tmp/release-1.3.9-before-8x。
- CLI/图片桥目标范围设1–8，插件两页列表一致；OutputScale配置属性将旧9–16钳制8，队列/图片/工作台共用值。只改目标输出范围，不改模型原生scale或用户能力记录。定向8边界/9拒绝及旧配置UI测试已加入，尚待运行。

### 2026-10-01 17:47 - Codex：8x修订本地验证完成

- 范围：两页选项原生/1–8，CLI及图片桥同步；PluginConfig.OutputScale用属性钳制0–8，旧JSON16实际反序列化为8，队列和图片用同一值。模型原生scale、Engine缓存、用户模型能力清单不变。
- 验证：ModelMetadata接受8并拒绝9/16/17，UI15项通过（JSON迁移、两页9个选项、8边界、既有状态同步），图片桥py_compile，build/publish0警告0错误；最终安装/回滚、自更新/独立组件保留、ZIP DLL/EXE和stable大小/hash通过。Backend29709文件审计UNCHANGED，不重跑GPU/下载/完整WiX压力矩阵；此前完整门禁沿用。
- 文件：PluginConfig、UpscalePage、OutputScale、CliHelp、图片桥、倍率/UI探针、能力审计文档、Notes及状态/中文版本记录。PluginConfig原文件已有混合换行，机械插入首次造成新增CRLF尾白提示，按Git原始字节仅替换属性块保留未改行，diff检查最终通过；据最终原编码源码重新publish并冻结资产，未在上传后重包。
- 原资产保存Artifacts/.refactor-tmp/release-1.3.9-before-8x；修订证据Artifacts/.refactor-tmp/release-1.3.9-8x。用户明确同版本覆盖，下一步提交推送main、用精确tag lease更新v1.3.9、clobber相同五资产并更新正文，ModelScope同路径--no-cache替换。已有旧1.3.9客户端SemVer不会发现同版本，需要手动安装；本机会同步更新。

### 2026-10-01 17:54 - Codex：1.3.9同版本8x修订覆盖收尾

- Authorization/Git：用户明确“改完之后替换1.3.9release”，本轮不增版本；源码b1cebcb正常推送main，v1.3.9注释tag用force-with-lease只更新该标签。旧tag对象60b132e记录在previous-tag-object.txt，新对象275161c指向b1cebcb；没有强推main。原资产/正文保留在release-1.3.9-before-8x。
- Remote：GitHub同一Release五资产clobber，正文与target_commitish更新；先四包再stable。ModelScope用隔离目录仅含1.3.9及清单/README/Notes，--no-cache替换，7项提交、0失败/0删除；Models备用EXE同路径同步。旧其他版本、模型权重和独立Backend资产未更换。
- Verification：11文件（GitHub5、ModelScope5、Models备用1）实际HTTP200、大小/SHA256均匹配冻结8x修订产物；两份stable JSON一致、分类正文一致，后端2026.09.30.1两条历史补丁保持。实际编译插件的GitHub检查失败→ModelScope清单及ModelScope包失败→GitHub新hash下载通过。
- Product：CLI和图片桥目标1–8，工作台/图片选择原生+1–8共9项，PluginConfig载入旧JSON16→8且两页/队列使用同值。15项界面检查、CLI8接受/9以上拒绝、图片桥py_compile通过；build/publish0警告0错误、最终安装/回滚与自更新/独立组件保留、ZIP/清单hash通过。Backend29709文件逐项审计UNCHANGED。此前模型结构、代表GPU与完整WiX门禁沿用，不重复完整GPU/下载/安装压力。
- Local：检查宿主/CLI退出，备份`C:/Codex Program/3fui plugin/Artifacts/.refactor-tmp/backup-3fui-before-1.3.9-8x-20261001-175231`后用最终修订EXE正式--apply-update返回UPDATE_COMPLETE|1.3.9；安装EXE/DLLhash匹配，-h输出范围1–8。配置、models/User/model-catalog.json与aria2-next.exe哈希保持，不自动改能力记录、不启动宿主。旧高倍率只在新程序载入配置时归一，未在部署脚本改写配置。
- Assets/evidence：新EXE17312244字节/hash98519edf…，安装器16091564/hash253b5c4b…，ZIP18611086/hash2efb7398…，stable1665/hash9bbe3253…，GPL源码hash不变；完整hash见版本记录及Artifacts/.refactor-tmp/release-1.3.9-8x。构建末尾仅为保持PluginConfig原有混合换行重publish，最终产物重新冻结并验证后才上传，上传后未重新打包。
- Limitation：同版本SemVer不会触发已装旧1.3.9的自动更新，其他设备需手动安装修订版；本机已替换。用户选择此同版本覆盖策略，未扩展修改更新器语义。
- Closeout：STATUS/中文进度/版本记录更新；main与tag已推送，最后提交推送收尾记录并核对干净工作树。此前独立TODO不变。
