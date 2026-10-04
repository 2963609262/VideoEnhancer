# Project Status

Last updated: 2026-10-04 22:28
Updated by: Codex

本文件是唯一 AI 操作状态来源。历史原文已冻结归档至 [记录归档](../archive/records-2026-10-01/README.md)，仅查历史时读取；归档中的版本、远端、环境和 TODO 不代表当前状态。

## Current Snapshot

- 2026-10-04 22:28：用户“这些做完后覆盖11”明确授权同版本修订1.3.11，取代临时1.3.12开发分流，版本源已恢复1.3.11。通用Bin组件SHA256安装标记、旧安装版本待核验、同路径旧完整包/旧续传缓存失效已实现。组件状态22场景、真实下载/标记及旧缓存替换通过；0警告0错误。先前1.3.11五资产留存Artifacts/release-1.3.11-before-component，WiX已clean避免同版本载荷缓存；修订发行构建/门禁进行中，随后覆盖源码标签/GitHub/MS及本机。RTX独立包仍2026.10.04.1，不覆盖其权重/运行库。

- 2026-10-04 22:19：**1.3.11正式发布完成**：[GitHub v1.3.11](https://github.com/maxzrb/VideoEnhancer/releases/tag/v1.3.11)、ModelScope Releases及Models备用EXE同步；源码/标签7ffeea2。RTX2026.10.04.1同步Models，源码6afb9a8/tag已推送。11本体远端实际下载hash+RTX归档hash+最新包筛选通过，双源故障回退通过；真实CLI覆盖旧RTX并记录版本通过。本机正式升级1.3.11及RTX，配置/用户清单/aria2/7zip保持，宿主未启动。用户新增要求：其他组件也须更新识别，随后独立实现，不修改已发布1.3.11资产。

- 2026-10-04 22:12：用户已授权发布1.3.11，覆盖之前仅本机试用限制。版本源与分类说明已更新；补RTX安装版本标记及下载页“可更新”入口，旧用户升级本体后刷新列表即可下载覆盖组件。RTX独立版本2026.10.04.1，源提交6afb9a8。发行五资产已冻结，本地publish/安装与回滚/自更新/Burn/队列/宿主/滚动/Python11项/组件状态4场景通过；Python独立归档仍2026.09.30.1，29,709文件审计UNCHANGED，尺寸脚本作为CLI内嵌资源同步，不上传新Python包。即将提交推送和发布双源，远端回读及本机正式升级尚待完成。

- 2026-10-04 21:47：AVV3官方PTH确认网络4x，目标2x来自后缩放；官方示例用CPU Lanczos4，项目TRT沿用图内双三次。更正NCNN“原生2/3x”：本项目2/3x图仍48通道卷积+PixelShuffle4，末尾Interp0.5/0.75，属于2/3x输出图，不是独立2/3x训练网络；待复核界面/清单用词。仅文档纠正，不修改已部署修复。

- 2026-10-04 21:30：**用户授权部署供3FUI测试，已备份并部署到本机Plugin目录。CLI/插件DLL/RTX组件与配套尺寸脚本共16项hash核对通过，配置/用户清单/aria2/7zip保持。真实安装CLI对FMP4样本RTX2x输出1280x720/4帧成功；宿主未启动，等待用户测试。仍1.3.10本地试用，不发布远端。**

- 2026-10-04 21:21：**模型目标尺寸与RTX解码回退修复完成，隔离构建/实机定向验收通过。TRT低倍率恢复图内双三次，CUDA及BasicVSR++张量侧Lanczos4留在GPU；NCNN/ONNX/Flash现有CPU输出在下游前调整。六后端、图片、两种补帧顺序、跨后端及TRT分块/高倍率通过；RTX五输入和完整CLI FMP4输出4帧通过，双显卡能力探测同步选择NVIDIA。源码差异及候选资产可审查，未部署/发布/改版本/提交/推送。**

- 2026-10-04 19:47：MPEG-4 Visual对照已复现RTX普通尺寸解码失败：FMP4 640x360 RTX首包Invalid argument/0帧；同源NCNN2x输出1280x720/4帧成功，低尺寸192x128 RTX软解输出384x256/4帧成功。三FourCC样本FFmpeg/OpenCV通过。sidecar仅按低尺寸软解，缺按编码能力回退；待确认用户失败后端/日志，详见[定位报告](../mpeg4-visual-decode-audit.md)，未实施修复。

- 2026-10-04 19:40：全后端补查完成。CUDA/NCNN/ONNX低目标同样原生推理后FFmpeg缩小，相比旧RVE队列前OpenCV缩小改变管道量/算法；先超后补可一直按原生高尺寸运行。FlashVSR2/4x直接推理，BasicVSR++官方4x仍后缩放。统一方案扩大至全部后端，详见审计报告；未实施产品修复或测速。

- 2026-10-04 19:37：全模型倍率路径审计完成，见[审计报告](../model-output-scale-path-audit.md)。98项清单中41项TRT（11项4x、28项2x、2项1x）共用通用转换入口，低倍率输出风险不限AVV3；视频/图片、分块及组合补帧须整体处理。FlashVSR显式2/4x保留，其他后端原生后缩放不能直接等同TRT旧版回归。尚未实施产品修复或逐模型速度测试。

- 2026-10-04 19:33：用户确认AVV3为TRT。发现旧版按后缀传目标倍率到convert_tensorrt.py，FinalOutputScale将4x后bicubic缩小编入GPU引擎；新版固定4x Engine→4x帧回传/管道→FFmpeg Lanczos缩小，存在可解释的性能退化路径，尚未同参数计时。FFmpeg本机包含RM/RV10–40；官方RMVB样本前缀4帧RGB解码通过，OpenCV首帧可读，但帧数报告偏大。四宫格漏rm/rmvb扩展名，RVE元数据依赖OpenCV，需定向修复；本轮未改产品代码。

- 2026-10-04 19:21：三条用户反馈完成代码核查：普通RVE由后端启动FFmpeg默认CPU解码并管道送帧，分段同样软解；RTX为独立sidecar，历史低分辨率软解已有专项修复，不能据此宣称所有编码都可回退。AVV3内置NCNN有原生2/3/4x，PTH登记4x；目标2x在固定4x模型后缩放。已询问失败后端/日志、模型格式及前后版本，未修改产品代码或发布。

- 2026-10-04 11:35：**1.3.10正式发布完成：[GitHub v1.3.10](https://github.com/maxzrb/VideoEnhancer/releases/tag/v1.3.10)、ModelScope Releases及Models备用EXE同步；源码/注释标签9bfba97已推送。PR9合并fb19abe，PR8仅aria2选择性提交5f1c9c0，原PR8已关闭。11远端实际下载hash、双源故障回退通过；本机正式升级1.3.10，配置/用户清单保持，aria2-next2.8.3。Backend继续2026.09.30.1、不动模型权重；发行收尾文档提交推送后核对干净工作树。**

- 2026-10-04 11:25：1.3.10主线整合与最终门禁完成，五项资产已冻结。PR8仅aria2选择性提交5f1c9c0，PR9合并fb19abe；build/publish0警告0错误、六类插件UI/宿主契约、下载队列5场景、Python路径16项、发布5/后端更新6、内外层安装回滚/自更新通过。后端逐文件UNCHANGED，准备推送main/v1.3.10并发布双源。

- 2026-10-04 11:15：用户反馈试用功能合理，明确授权选择性合并PR8/PR9并发布1.3.10；覆盖先前不正式合并/不发布限制。PR8仅aria2提交5f1c9c0，PR9原版ae94b26无冲突合入；保留倍率语义和WiX。准备正式发行门禁、双源发布与本机升级。

- 2026-10-04 10:46：用户已升级3FUI至6.2.35/LakeUI5.110；PR8仅aria2-next2.8.3版本/固定哈希/源码与许可打包选择性移植到main，不合并倍率或安装交互。PR9精确ae94b26在Artifacts/pr9-review隔离组合publish通过，已备份后部署；EXE/DLL/aria2哈希一致，配置/用户能力清单保持。PR9未合并，未发布/提交，等待实际试用反馈。详见[审核报告](../pr8-pr9-review.md)。

- 2026-10-01 17:54：**1.3.9已按授权替换为8x修订版**。目标范围1–8，旧9–16x配置载入为8；源码与v1.3.9标签`b1cebcb`已推送，五资产/稳定清单/分类正文及两处ModelScope同步。11项实际下载哈希、故障回退通过，本机同版本替换成功且配置/用户能力清单/aria2保持。已安装旧1.3.9可等后续版本正常更新；修订及收尾记录已推送，工作树干净。

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

- [ ] 倍率术语复核：区分学习网络倍率与已编译图输出倍率；AVV3 NCNN2/3x实际为4x网络+图内缩小，之前“原生2/3x”表述不准确。现有图输出尺寸/缓存行为保持，尚未改产品元数据或界面。

- [x] 用户确认RTX超分；已修复解码能力判断、硬解初始化/首帧失败回退、平面YUV转换及双显卡能力误判。RTX五输入及完整CLI FMP4通过；用户原片仍未取得，不扩大为所有老编码变体验收。

- [x] 全后端模型目标尺寸修复：TRT图内输出/缓存、CUDA/BasicVSR++原设备Lanczos4、CPU封装结果缩放、视频/图片/两种补帧顺序/跨后端/分块同步完成并定向验证。未做全模型逐项或旧新整链性能比较。
- [x] 正式分发1.3.11：本体双源/RTX独立包、远端hash、更新回退、本机正式升级完成。
- [ ] 通用组件更新识别已实现：待按明确授权覆盖1.3.11，并完成修订双源hash/本机升级。
- [ ] 老视频入口后续工作：四宫格补.rm/.rmvb、RVE元数据改可靠FFmpeg探测；不属于已确认RTX MPEG-4 Visual解码修复，尚未实施。

- [x] PR9+aria2组合已本机部署，用户反馈功能合理并授权合并/发布；不把该反馈扩大为全部GPU或历史专项验收。

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

- 2026-10-04 21:30：用户“部署到3fui我测试”明确授权本机覆盖部署，取代21:21未部署状态；不扩大为远端发布。先备份、检查进程退出，再部署并hash核对，保留配置/模型/独立下载组件。

- 2026-10-04 21:21：用户授权执行全部模型后端与RTX修复，并明确尽可能保持GPU处理。GPU Lanczos通过张量运算实现，不将CUDA一概改成双三次；TRT低倍率沿用旧转换图内双三次。保留真实权重倍率、Flash真实2/4x及分段独立倍率语义。NCNN/ONNX封装、Flash分块合并现有CPU边界如实保留。

- 2026-10-04 11:35：用户“功能合理，选择性安全合并并发布1.3.10”授权正式合并/推送/发行，取代此前仅本机试用限制。PR8保留aria2部分、排除倍率预设和控制台安装逻辑；PR9已合并。运行要求LakeUI5.110+（5.x），旧aria2自动更新时保持，需要安装器/手动包升级独立组件。

- 2026-10-04 10:46：用户授权仅选择性移植PR8的aria2升级并部署PR9组合，不整包合并PR8、不正式合并PR9，版本保持1.3.9、不发布远端。

- 2026-10-01 17:56：用户确认1.3.9刚发布，同版本替换影响小；旧1.3.9用户可等待下一版本正常更新，无需当前手动替换，不增加同版本自动更新机制。

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

- 2026-10-04 21:30当前安装为1.3.10模型/RTX修复本地试用：CLI 25d20aba6475092b57fa267fbeab7aac873429f5f8ba63d54a390aecdb76b296，插件DLL 7398496759b5d9fa7eadff2b4fa4f5edec63281cce706c6f6a8afffc63eb5392，RTX b64761cd06f58517755964f5b6f797152c4abab14b31329057558028e0bcb844。备份 C:\Codex Program\3fui plugin\Artifacts\.refactor-tmp\backup-before-model-rtx-fix-20261004-212715；证据Artifacts/model-fix-build/local-deployment.json。下方正式版本hash为部署前历史。

- 2026-10-04 21:21本轮继续同设备：.NET10.0.400、MSVC14.51、Visual Studio18生成器；RTX3060 Laptop实机。隔离运行目录Artifacts/model-audit/runtime的backend是实际副本，python解释器及models部分为已核查链接；只更改项目内backend/测试文件，不覆盖真实安装。独立RTX源码位于Artifacts/rtx-backend-fix，基准c83df0f；FFmpeg/RTXSDK/CMake依赖使用本机已有只读位置，下次换设备重查，不作为便携配置。

- 2026-10-04 11:35当前本机正式1.3.10（3FUI6.2.35/LakeUI5.110），EXE hash `ac50014afde5b3e8f0181350f3c4f7ff079d72de15ec40de20d050b539516ac1`，DLL `c7f2fa9988d4941feb29a2b86e3977250c8f50e5032196c421261f96b953e1bd`，aria2-next2.8.3。备份`C:/Codex Program/3fui plugin/Artifacts/.refactor-tmp/backup-before-release-1.3.10-20261004-112800`；证据Artifacts/.refactor-tmp/release-1.3.10/local-deployment.json。以下旧试用/发行环境条目仅为历史参考。

- 2026-10-04 10:46：当前实际宿主6.2.35/LakeUI5.110，本机部署为PR9+aria2-next2.8.3试用1.3.9；下述旧HostBin/发行hash仅为历史。PR9构建无需HostBin。备份`C:/Codex Program/3fui plugin/Artifacts/.refactor-tmp/backup-before-pr9-aria2-20261004-104341`，证据`Artifacts/pr9-review/Artifacts/local-deployment.json`。
- 当前试用EXE SHA256 `2534d4fbb916374b68b262a87124af4c2a987a4264441e5cfdd7b21bb089108c`，DLL `139e74127b08518f3078eacb9c507098b7c9c121dd017e5be8074befb0c83bcb`，aria2 `08afaf2a44811d38e7ce538da719ab06d6925bcaad1231ee7b92c497f58e5aac`。

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

- 2026-10-04 21:21：git pull --ff-only已最新，main=0664186跟踪origin/main；本轮源码/测试/两份审计报告/RTX补丁/记录未提交，工作树非干净，无push。Artifacts内构建/模型/测试夹具不纳入Git，切换工具或设备前建议考虑提交。

- 2026-10-04 11:35：PR8选择性5f1c9c0、PR9合并fb19abe、发行9bfba97及v1.3.10已推送。PR9状态MERGED，PR8选择性移植后CLOSED；不整包合并PR8。后续仅收尾文档提交，main与tag正常推送、无强推；冻结资产不纳入Git。

- 2026-10-04 10:46：main=5942b48同步origin/main；aria2选择性代码/许可/发布流程及审核记录未提交，工作树非干净。PR9 detached ae94b26仅附加同样aria2改动；未推送。

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

### 2026-10-01 17:56 - Codex：确认旧1.3.9迁移策略

- 用户说明1.3.9刚发布，直接替换影响小；旧1.3.9等下一版即可。更新当前快照、持久决策及版本迁移说明，撤回前述要求立即手动更新的建议；保留历史日志说明当时记录。
- 仅文档修改，不改程序/资产/标签，不重跑构建或发布；git pull已最新，起始main干净。UTF-8和原换行保留，diff检查后提交推送文档，结束核对工作树。

### 2026-10-04 10:32 - Codex：PR #8/#9 审核与原版构建

- 读取AGENTS/INDEX/STATUS及HandShake，新会话；git pull --ff-only已最新，main=5942b48。gh默认识别upstream，发现后显式-R maxzrb/VideoEnhancer并核对URL，最终审核本仓库PR。
- 获取origin/pr-8及pr-9，项目Artifacts/pr9-review隔离检出ae94b26；原版publish成功，host-runtime（含真实宿主契约）、DPI、scroll通过；不重复GPU/下载/安装器全矩阵。
- 从实际单文件宿主只读提取FFmpegFreeUI/LakeUI到Artifacts/pr9-host-check，版本6.2.33/5.109；PR要求5.110，已向用户询问试用策略，未部署。PR8倍率方案被当前输出倍率覆盖、安装交互违背已确认方向；aria2可单独考虑。只读merge-tree检出5处冲突。
- 新增docs/pr8-pr9-review.md；main仅审核和状态记录变化，无源码合并、版本/远端发布或GitHub评论。待用户选择后完成本机备份部署和收尾。

### 2026-10-04 10:46 - Codex：aria2选择性移植及PR9本机部署

- 同工具续作，复读AGENTS/INDEX/STATUS并沿用HandShake；git pull --ff-only已最新，保留审核记录。用户明确授权只合aria2并试用PR9。只读提取升级宿主确认6.2.35/5.110，原兼容阻碍解除；GitHub API核对aria2 tag653e070b→f58a2d9和Windows官方digest。
- main与隔离PR9移植csproj的aria2版本/固定二进制与源码hash、源码下载/产物、依赖许可验证及两种安装载荷打包；SOURCE、THIRD-PARTY-NOTICES和DEPENDENCY-LICENSES同步，发布流程示例源码名更新。未引入倍率预设、convert_tensorrt、InstallerManager/InstallerBundle变化或移除WiX。
- 组合dotnet publish成功，插件/安装器0警告0错误；二进制和源码SHA256匹配，COPYING/AUTHORS与源码一致，13项依赖许可规范行末空白后相同；ZIP15项中的DLL/EXE/aria2/依赖许可与最终文件一致。aria2 --version2.8.3，实际下载GitHub stable.json1665字节/hash9bbe3253…与官方digest一致。升级后真实宿主契约通过，前轮DPI/scroll仍有效，未重跑GPU/全量安装器矩阵。
- 实施纠正：首个移植脚本在试用树工作目录执行，仅改试用树后第二路径不存在，改在项目根明确同步main；初次依赖许可严格比较失败原因仅行末空白，检查后规范比较通过；记录脚本初次JSON路径键使用正斜杠而证据含反斜杠，规范键后更新成功。均未影响安装结果。
- 宿主/CLI退出后备份`C:/Codex Program/3fui plugin/Artifacts/.refactor-tmp/backup-before-pr9-aria2-20261004-104341`，最终EXE正式--apply-update返回UPDATE_COMPLETE|1.3.9；自更新保留独立组件，随后显式替换aria2及随附许可。EXE/DLL/aria2哈希一致，配置及models/User/model-catalog.json哈希保持；证据Artifacts/pr9-review/Artifacts/local-deployment.json。未启动宿主、不动模型/Backend、不发布或变更版本。
- 收尾记录更新；main选择性代码和文档未提交，工作树非干净，PR9未合并。等待用户实际界面/真实任务反馈，建议考虑提交aria2选择性升级。

### 2026-10-04 11:15 - Codex：1.3.10整合与发行启动

- 同工具续作，复读AGENTS/INDEX/STATUS和发布流程，沿用HandShake；git pull --ff-only已最新。用户授权提交推送、选择性整合、1.3.10双源正式发行及流程内本机升级。
- PR8仅aria2相关代码/许可/记录提交5f1c9c0，未带入旧倍率和控制台安装。PR9合并ae94b26无冲突；比较已试用源码，仅aria2源码产物路径自动合并仍用旧变量，改ResolvedArtifactsDirectory恢复试用版本。PR9合并提交保留原分支 ancestry；PR8以选择性移植结案。
- GitHub/ModelScope认证可用，v1.3.10不存在，SDK10.0.400及磁盘空间可用。继续构建/必须发行门禁→提交标签/上传冻结资产→双源哈希回读/故障回退及本机部署，不扩大GPU全矩阵。

### 2026-10-04 11:25 - Codex：1.3.10本地门禁与冻结产物

- Release与插件/CLI版本一致，六类插件检查（宿主契约/DPI/scroll/appearance/tooltips/倍率）、DownloadQueueUi5场景通过；Python16项路径/控制静态检查通过。旧测试仍要求外部宿主Private=false/ExternallyResolved，修正为LakeUI编译包/不带运行库和无宿主Reference，非产品代码故障。
- build/publish0警告0错误；发布门禁5与后端更新6、内层安装/无效目录/回滚、自更新独立组件保留/回滚、最终WiX成功/无效目录/回滚及缓存/临时登记清理通过。ZIP15项，DLL/EXE/aria2/依赖许可hash一致，不含宿主/渲染DLL或外置布局JSON。不重跑完整GPU矩阵。
- 独立后端按上一发行目录逐项SHA256审计UNCHANGED，继续2026.09.30.1。aria2源码下载遇EOF且停滞，终止自有dotnet发布进程，改为存在源码缓存时复用并始终锁定hash；发布脚本复用构建产物再hash，防止重复下载。缓存恢复自已验证41,412,730字节归档，最终hash420e3125…匹配。重新完整构建成功，首次失败日志保留。
- 冻结五项资产hash在Artifacts/.refactor-tmp/release-1.3.10/asset-hashes.json；上传后不重包。接下来提交/推送/标签、GitHub及ModelScope同步、11文件实际hash和双源故障回退、本机正式升级。

### 2026-10-04 11:35 - Codex：1.3.10双源发行与收尾

- Git：main已推送5f1c9c0(aria2选择性)、fb19abe(PR9有祖先关系合并)、9bfba97(release1.3.10)，注释tagv1.3.10同指向9bfba97。PR9自动标记MERGED；PR8因排除其余行为仅关闭，不伪装整包合并，不发送评论。
- GitHub正式latest五资产及逐行分类正文正确；ModelScope隔离上传只含新版本/README/Notes/stable，7项提交、0删除，Models备用EXE同路径--no-cache复用hash。冻结后未重包，旧发行/模型/Backend channel不变。
- 验证：GitHub5/ModelScope5/Models1共11文件实际HTTP200、大小/SHA256与冻结资产一致，两份stable相同；模型96项，无PotPlayer/重复，备用EXE唯一且17,316,043字节。Backend线上2026.09.30.1与两条补丁保持，本地逐项审计UNCHANGED。
- 网络：首次Python下载GitHub资产read-timeout、插件包回退TLS握手EOF；ModelScope六文件当时通过。只重试GitHub失败项，用Windows curl实际下载通过，结果remote-verification.json11/11；插件故障回退先临时进程IPv6关闭重试成功（DNS本无IPv6），随后撤掉该变量在默认环境再次通过。没有关闭TLS验证或修改产品网络逻辑，原失败日志保留。
- 实际更新器：GitHub检查仓库404→ModelScope1.3.10；ModelScope包数据集404→GitHub1.3.10包且hash通过。夹具在项目Artifacts，不改真实配置。最终六类UI、队列5场景、Python16项、release5/backend6、内外层安装回滚/缓存清理与自更新通过；未重复GPU全矩阵，用户此前实际功能反馈合理。
- 本机：确认宿主/CLI退出，备份`C:/Codex Program/3fui plugin/Artifacts/.refactor-tmp/backup-before-release-1.3.10-20261004-112800`后正式--apply-update返回UPDATE_COMPLETE|1.3.10；显式补齐aria2/许可，EXE/DLL/aria2hash匹配、插件配置及models/User/model-catalog.json保持。未自动启动宿主，版本记录/中文进度/审核报告更新。
- 证据：Artifacts/.refactor-tmp/release-1.3.10中的asset-hashes、github-release、remote-verification、remote-models、remote-backend-channel、local-deployment、build-release、门禁/UI日志及updater-probe。长期TODO保持，不扩大本轮任务。完成收尾文档提交推送后核对干净工作树。

### 2026-10-04 19:21 - Codex：解码与AVV3倍率反馈核查

- 新会话，使用HandShake，读取AGENTS/INDEX/STATUS，git pull --ff-only已最新，起始main=0664186、工作树干净。
- 核查：cli/Program.cs BuildBackendArgs将原视频路径与FFmpeg路径传给RVE；部署后端src/FFmpegBuffers.py与项目Artifacts/model-audit/runtime副本SHA256一致（a8077454…），FFmpeg读取命令无hwaccel，CPU解码为rawvideo并通过stdout供模型使用。分段桥同样使用无hwaccel的FFmpeg解码管道，不要求预先转码。
- RTX与RVE分开：归档记录2026-09-14已对宽<320或高<240走软解/上传D3D11且边界专项通过，但本轮未检查sidecar完整源码或复现老视频，不能推断所有编码通用软解已支持。需用户失败后端、输入编码和错误日志再定位。
- AVV3：cli/model-capabilities.json明确NCNN 2/3/4x三个条目，PTH realesr-animevideov3为4x；DetectScale优先能力清单/图结构/检测，固定模型目标倍率通过OutputScale.Encoder在编码前Lanczos缩放。本地Compact实现由末层权重形状推导PixelShuffle倍率；改名不改变该PTH结构。用户旧版仅改后缀的行为尚未复现，不能据此否定其性能反馈。
- 下一步：待用户提供AVV3文件格式/模型名、旧新版本、软解失败时后端/日志，区分原生2x识别问题与4x后缩放性能问题，再形成定向实现方案；不恢复全局文件名覆盖权重倍率。
- 验证仅源码检查/部署后端文件hash核对，未运行GPU/性能测试，不宣称速度下降原因已确证。更新STATUS和中文进度，版本记录不变；没有提交/推送/部署/发布。本轮只新增两份记录修改，建议切换工具前考虑Git提交。

### 2026-10-04 19:33 - Codex：TRT图内缩放与旧编码兼容性核查

- 同工具继续HandShake，复读STATUS快照/最近日志，沿用AGENTS/INDEX；git pull --ff-only已最新，main=0664186，保留上一轮两份未提交记录。
- 用户澄清AVV3为TRT。核查v1.3.8 DetectScale文件名倍率解析、EnsureTensorRtEngine的outputScale参数以及现版模型检测/OutputScale.Encoder。部署convert_tensorrt.py与项目副本SHA256一致（db4d1187…），FinalOutputScale明确将模型输出用F.interpolate bicubic缩放后再导出；目标2x/3x的Engine可由同一原生4x权重构建，不是内嵌三套原生权重。
- 性能解释：旧版改后缀可将2x传入转换脚本、生成GPU图内缩放的2x输出Engine；新版目标倍率与原生倍率分离后，固定4x权重默认生成4x Engine，再在FFmpeg Lanczos缩小，向CPU/编码管道输出4x帧，像素数为2x输出的4倍。该变化具有明确源码依据，尚未用用户旧新参数计时，不宣称模型卷积计算量变为4倍或性能根因已实测锁定。普通RVE覆盖倍率为CPU OpenCV INTER_AREA缩小；TRT转换特例不同，纠正中间GPU缩放泛化推测。
- 在线主源：Real-ESRGAN官方inference_realesrgan.py明确AVV3 upscale=4；FFmpeg n7.1 allcodecs.c有RV10/20/30/40，allformats.c有RM解复用。实际本机兼容FFmpeg -decoders/-demuxers列出RM、RV10–40、MPEG1/2/4、WMV1/2/3及Cook。宿主Settings工作目录为空导致首次枚举命令失败，随后显式使用已知后备FFmpeg，未修改设置。
- 定向样本：从https://samples.ffmpeg.org/real/VC-RV40/spygames-2MB.rmvb下载，45秒超时仅887800/2097152字节前缀；保留证据、不当完整文件。前缀ffprobe为rv40/yuv420p/576x320；FFmpeg成功解4帧rgb24共2211840字节，等于576*320*3*4；部署Python OpenCV打开/首帧成功、fps23.9760437、报告帧数174722，不能作为实际完整帧数。结果Artifacts/feedback-decode-audit/result.json，未进入Git。
- 兼容性：模型接收RGB像素/张量而非RMVB压缩码流，编码支持取决于解码器。RVE RenderVideo实际使用OpenCVInfo读尺寸/fps/帧数，VideoInfo/RenderVideo部署文件hash与项目副本一致；老编码可能在此先失败或进度异常。QuadGridForm IsVideoFile和文件筛选漏.rm/.rmvb，SetVideo明确拒绝，因此该入口存在独立限制。没有用户原片，不能据此认定其具体报错原因。
- 下一步方案：保留原生4x元数据，对TRT低倍率输出恢复显式目标倍率的图内缩放及按实际输出倍率缓存；兼顾分块、组合补帧和画质算法差异，不恢复全局改名倍率覆盖。老视频优先补入口扩展名并替换OpenCV元数据依赖为可靠FFmpeg探测；用用户原片日志验证。当前用户要求调查，未实施/发布/部署/改版本。
- 记录：STATUS快照/待办及中文进度更新；验证仅上述源码/hash/4帧解码，未运行GPU矩阵/速度对比。本轮两份记录未提交，git diff --check通过，工作树非干净；建议考虑提交后切换工具。

### 2026-10-04 19:37 - Codex：其他模型倍率风险扩展审计

- 用户担心其他模型同类问题，同工具沿用HandShake/AGENTS/INDEX，复读STATUS快照和最近日志；git pull --ff-only已最新，保留两份记录修改，main=0664186。
- 全清单机器统计98项，其中TRT41项=4x11/2x28/1x2。检查普通视频、图片、同后端/跨后端组合、分段、FlashVSR及RTX的入口和缩放位置；新增docs/model-output-scale-path-audit.md，列出全部41项TRT候选范围和条件。
- 确认低目标倍率引擎构建/编码后缩放为通用路径，并非AVV3特判；TRT图片同类。先超后补目标只在编码时生效可导致补帧仍按原生高尺寸运行；需同步帧/预览/补帧缓存及分块拼接。FlashVSR声明2/4x已有直接推理选择；其他后端与TRT图内缩放回归区分，未宣称全部模型实际变慢。
- 下一步为统一TRT原生/Engine输出/目标倍率管理、缓存隔离和视频图片同步，涵盖分块/组合补帧及算法差异，不做AVV3名称补丁。用户本轮为风险核查，未实施产品代码修复，不改版本或发布/部署。
- 验证：清单JSON读取与逐项统计、源码路径核对及git diff --check；未进行GPU矩阵/逐模型性能测试。本轮报告与两份记录未提交，工作树非干净，建议考虑提交后切换工具。

### 2026-10-04 19:40 - Codex：CUDA/NCNN/ONNX及时序后端倍率核查

- 用户要求范围不限TRT，同工具沿用HandShake，复读STATUS/前序审计，git pull --ff-only已最新，保留已有三份文档修改，main=0664186。
- 逐后端统计CUDA47(1x2/2x33/4x12)、NCNN21(1x2/2x13/3x1/4x5)、ONNX28(1x2/2x23/4x3)、FlashVSR1、BasicVSR++1；声明可重叠，不作为额外唯一模型总量。
- 核对v1.3.8覆盖倍率参数、部署RenderVideo/Util普通CPU INTER_AREA缩放，以及主线目标只进FFmpeg滤镜，确认其他普通后端也存在位置/算法变化和原生高尺寸管道量。GPU→CPU是否增加须区分：普通旧路径也先回CPU，TRT旧版图内缩放单独支持；未泛化为全部旧版GPU缩放。
- 核对FlashVSR dispatcher将override scale传给runner并传node.main，2/4x为直接推理；其他目标仍后缩放。BasicVSR++根据is_low_res_input确定4/1x，原生片段numpy输出后进入RawVideoWriter，官方4x低目标须后缩放。
- 核对同后端先超后补按upscaleTimes初始化并在模型结果后执行override尺寸，跨后端先超后补中间视频无目标滤镜，当前低目标只最终编码生效，可增加补帧面积；先补后超不同。图片通用Pillow后缩放、时序桥原生输出也核查。
- 更新审计报告全后端表与统一方案；修复范围扩大为模型原生倍率、处理链帧尺寸、目标尺寸统一管理，各后端在后续处理前缩放一次，尽可能GPU侧，否则至少减少管道量；保留真实多倍率和算法差异，不只修TRT。
- 本轮源码审计/JSON统计，未改产品或运行GPU/性能测试；STATUS和中文进度已更新，git diff --check通过。报告与两份记录未提交，工作树非干净，建议考虑提交后切换工具。

### 2026-10-04 19:47 - Codex：MPEG-4 Visual/FMP4实际解码对照

- 用户提供失败源编码MPEG-4 Visual/XviD/DivX/FMP4，同工具沿用HandShake及AGENTS/INDEX；复读STATUS最新状态/日志，git pull --ff-only已最新，main=0664186，保留前序文档修改。
- 三个4帧640x360合成样本，FFmpeg mpeg4+FMP4/XVID/DIVX FourCC、yuv420p。部署后备FFmpeg和实际宿主FFmpeg8.1.1软解均成功，各RGB24字节2764800，OpenCV均正常640x360/24fps/4帧并首帧成功；不是用户原片或全部XviD特性矩阵。
- 实际安装CLI1.3.10：FMP4640x360普通NCNN AVV3-2x成功，输出1280x720/4帧；同源RTX VSR2x处理0帧、退出1、decoder send packet Invalid argument。192x128 FMP4低尺寸RTX成功，输出384x256/4帧。首次NCNN后端已完成渲染、CLI对既有Vulkan退出阶段异常有提示，未将其误认成此次解码失败，最终输出帧数完整。
- 读取对应已发行sidecar源码c83df0f：回退仅width<320或height<240，普通尺寸choose_d3d11_format无D3D11格式时返回NONE，没有编码能力回退。在线主源与本机对照支持RTX路由问题；用户失败后端尚未确认，已询问是否RTX/HDR及错误日志，不把样本复现直接等同用户原片。
- 网络：raw源码下载502，用gh contents API获取base64源码副本成功，未更改远端。诊断捕获CLI含混合控制台编码，保存UTF-8日志；关键英文错误/JSON与输出探测可读，不据乱码推断业务失败。
- 来源/证据：docs/mpeg4-visual-decode-audit.md；Artifacts/feedback-decode-audit的源码副本、mpeg4-decode-results.json、mpeg4-pipeline-results.json、三项实际处理日志与输出；产物不入Git。直接FFmpeg/模型验证与CLI输出均使用项目Artifacts，不修改用户配置或视频；CLI自身沿用已安装运行日志机制。
- 方案：独立RTX runtime按解码器硬解能力选择软解/上传，不仅按尺寸；初始化失败需明确重开软件解码边界，像素格式转换范围另检查。普通后端有问题则先看原片/日志而非添加模型编码限制。
- STATUS/中文进度和新增报告已更新；无产品修改、版本/发行/部署，不重跑完整模型矩阵。git diff --check通过，当前四份文档未提交，工作树非干净，建议考虑Git提交后切换工具。


### 2026-10-04 21:21 - Codex：模型结果端尺寸与RTX软解回退实施、实机验收

- 同工具继续HandShake，已复读AGENTS/INDEX/STATUS/skill，git pull --ff-only已最新，main=0664186；保留前序审计和未提交改动。用户明确授权执行修复，随后要求Lanczos尽可能留GPU。未请求重复批准，不扩展到发布或真实安装部署。
- 模型链：Program.cs把低目标输出传给TRT Engine构建/缓存，图片同步；普通模型统一走rve-ordered包装器，在结果进入插帧/预览/写队列前落实尺寸；独立精度对齐保留。新增rve_output_scale.py原设备Lanczos4，逐瓣累加限制临时显存；NCNN/ONNX的CPU输出用cv2 Lanczos4。Flash/Basic runner以内嵌脚本随CLI同步，Flash2/4x真实推理保持，Basic在回CPU前缩放。OutputScale.Encoder保留编码端最终尺寸兜底支持用户滤镜。
- 先补后超修复转场重复Frame共享导致原地改写，逐帧clone并按生成器顺序处理；新增共享帧回归。模型原生能力清单/权重不变，分段模式不套普通-output-scale。Flash原有CPU分块合并、NCNN/ONNX原生回传没有伪称消除。
- RTX：Artifacts/rtx-backend-fix中cpp枚举AVCodecHWConfig，硬解初始化或首帧失败重开软解；保留首帧探测所有流packet重放，软件帧转NV12/P010上传D3D11；补常见平面422/444/高位深。完整CLI最后发现双显卡capabilities探测默认集显误判，已对RTX/NVENC探测优先选择NVIDIA，与实际处理一致。可审查两文件diff存release/patches/rtx-video-decode-fallback.patch，reverse apply --check通过，README注明rve-patches/c83df0f。
- 验证：solution Release build/publish0警告0错误；GPU Lanczos2项（含4种目标尺寸、FP16设备/常量）通过，9项顺序/精度/错误退出测试通过（错误夹具traceback为预期），ModelMetadata参数/最终滤镜验证通过。RTX已有16项FfmpegTranscodePipeline单测通过。Python脚本语法编译通过，git diff --check通过。
- 实机模型：verify_model_catalog_runtime.py --scale-fix-only共17场景记录通过，六后端/图像/两顺序/跨后端/2x Engine；追加TRT32分块2x及目标6x、最终两顺序、完整RTX CLI共5项通过。测试先把2倍RIFE输出误预期8帧，源码与实际对照是4源帧+3间隙=7帧，已改断言，不改产品尾帧语义。新断言检查精确帧数。
- RTX新构建5种输入通过：FMP4 640x360、H264 high10/444/低尺寸软解上传；普通H264保持D3D11VA。输出均4帧、音频帧数不丢；能力JSON VSR/TrueHDR/H264NVENC/HEVCMain10为true（3060不支持AV1 NVENC）。最终CLI原调查FMP4输出1280x720/4帧成功。证据Artifacts/rtx-decode-fix-verification及model-audit/verification。不是所有历史XviD/DivX变体或用户原片验收。
- GPU缩放单步测速：3060 Laptop 4K FP16→1080p预热3/测10次平均11.793ms，峰值分配314154496字节，FP16相对CPU FP32最大误差0.0004884；只测缩放，未承诺整链提速比例或恢复用户旧版全部速度。Lanczos4不等同FFmpeg/Pillow参数/抗锯齿逐像素结果。
- 交付：Artifacts/model-fix-build内CLI、插件ZIP、安装器及rtx-video-runtime.zip，hash清单model-rtx-fix-assets.json。RTX源码补丁含在组件ZIP，相关报告更新；版本仍1.3.10，版本迭代记录不变。无提交/推送/部署/发布，实际安装保持原样。完成本轮授权修复，后续发布或部署再沿用这些可审查产物；建议考虑Git提交。


### 2026-10-04 21:30 - Codex：按用户授权部署3FUI试用

- 同工具续做，已复读AGENTS/INDEX/STATUS/HandShake启动规则；git pull --ff-only已最新，main=0664186，保留已有源码/测试/文档未提交改动。用户明确要求部署，无需另行确认。
- 本机进程检查：部署前后均未发现FFmpegFreeUI/3FUI/videoenhancer/vsr_backend运行。实际Plugin目录 C:/Program portable/3FUI/3FUI/Plugin，python/backend为实际目录。候选4资产逐项SHA256匹配model-rtx-fix-assets.json，手动ZIP内CLI与候选EXE一致。
- 备份：C:\Codex Program\3fui plugin\Artifacts\.refactor-tmp\backup-before-model-rtx-fix-20261004-212715。保存16项目标部署前文件及用户配置/模型清单/下载组件，deployment-before.json记录原hash和原不存在文件。项目内deploy-local.py只写明确Plugin目录内目标，未删除/搬动原目录。
- 部署：ZIP中的CLI及plugin/videoenhancer.3fui.dll；独立RTX包中的runtime EXE/DLL（DLL既有hash相同）；image/Flash/Basic/尺寸模块及1.3.10 embedded-tools下ordered/尺寸模块。16项目标hash匹配，用户videoenhancer.plugin.json、models/User/model-catalog.json、aria2-next和7za hash不变，权重不修改，未自动启动宿主。
- 验证：部署CLI -h为1.3.10；使用已部署CLI和RTX组件对项目FMP4 640x360样本运行2x、FFV1最终编码，成功输出1280x720、完整4帧。日志deployment-rtx-smoke.log、输出及local-deployment.json在Artifacts/model-fix-build；部署后再次逐项hash和保护文件核对通过。不重跑六后端GPU矩阵。
- 状态：本机试用已落实，等待用户从3FUI测试实际素材。项目/Backend远端版本和发布资产不变，无push/发布；STATUS、中文进度和两份审计的部署说明同步。工作树非干净、改动未提交，建议测试反馈后考虑Git提交。


### 2026-10-04 21:47 - Codex：AVV3官方倍率与NCNN图结构再核对

- 用户询问官方是否没有原生2x可选。沿用HandShake/当前部署状态，git pull --ff-only已最新、工作树保留既有未提交改动。仅查源和文档，不重跑GPU或修改部署。
- 在线主源：Real-ESRGAN docs/anime_video_model.md标X4（可用于1/2/3x）；inference_realesrgan.py的AVV3明确upscale4/netscale4；utils.py的outscale不同则cv2.INTER_LANCZOS4。确认官方这份PTH为4x网络+输出缩放；项目TRT GPU双三次不是官方具体算法指定。
- 实际读取项目隔离模型2x/3x/4x.param尾部：2/3x均Conv48通道、PixelShuffle4、Nearest残差4x相加后Interp0.5/0.75，4x无末端Interp。纠正先前把NCNN2/3x输出图称为原生2/3x网络的说法；官方NCNN README及src/main.cpp仅证明2/3/4x输出模型选择。没有证据把同一4x权重改后缀变成独立2x训练网络。
- 审计报告新增官方来源和图结构说明，STATUS快照/TODO与中文进度同步。当前模型图输出倍率检测及已部署TRT低倍率修复仍有效；新增术语复核TODO，不擅自改产品行为。无版本/发布/部署变更，无提交/推送；建议试用反馈后考虑Git提交。


### 2026-10-04 22:12 — Codex继续：1.3.11发行门禁与RTX组件更新入口

- 2026-10-04 22:12：用户已授权发布1.3.11，覆盖之前仅本机试用限制。版本源与分类说明已更新；补RTX安装版本标记及下载页“可更新”入口，旧用户升级本体后刷新列表即可下载覆盖组件。RTX独立版本2026.10.04.1，源提交6afb9a8。发行五资产已冻结，本地publish/安装与回滚/自更新/Burn/队列/宿主/滚动/Python11项/组件状态4场景通过；Python独立归档仍2026.09.30.1，29,709文件审计UNCHANGED，尺寸脚本作为CLI内嵌资源同步，不上传新Python包。即将提交推送和发布双源，远端回读及本机正式升级尚待完成。

- 修改：两版本源、release-notes.txt、DownloadInstallStatus.vb、ModelDownloadPage.vb、ModelDownloadManager.cs；保留本轮全后端/RTX修复。旧版无标记视作可更新，仅成功校验解压后记录版本；pending继续阻止误判。
- 验证：Artifacts/release-1.3.11内package/burn/release-gates/backend-updater/queue/queue-ui/host-runtime/scroll/python日志；组件状态probe通过旧无标记/旧版本/新版本/pending四场景。既有真实GPU和RTX五输入结果沿用，不重复全矩阵。
- Git：main同步0664186，工作树仅本轮预期修改；准备发行提交及标签。下一步上传RTX及本体双源、实际下载校验和本机正式更新。


### 2026-10-04 22:19 — Codex：1.3.11正式发行收尾

- 2026-10-04 22:19：**1.3.11正式发布完成**：[GitHub v1.3.11](https://github.com/maxzrb/VideoEnhancer/releases/tag/v1.3.11)、ModelScope Releases及Models备用EXE同步；源码/标签7ffeea2。RTX2026.10.04.1同步Models，源码6afb9a8/tag已推送。11本体远端实际下载hash+RTX归档hash+最新包筛选通过，双源故障回退通过；真实CLI覆盖旧RTX并记录版本通过。本机正式升级1.3.11及RTX，配置/用户清单/aria2/7zip保持，宿主未启动。用户新增要求：其他组件也须更新识别，随后独立实现，不修改已发布1.3.11资产。

- 本机证据Artifacts/release-1.3.11/local-deployment.json，备份C:\Codex Program\3fui plugin\Artifacts\.refactor-tmp\backup-before-release-1.3.11-20261004-221638。远端与升级证据verify-remote.log/remote-verification.json/remote-rtx.json/fallback-probe.log/rtx-upgrade.log/local-rtx-update.log。
- PythonBackend仍2026.09.30.1/channel历史2条补丁；GPU和RTX既有验证沿用。首个远端校验使用便携Python缺truststore，切换本机发布Python后11项通过。
- Git：源码7ffeea2、v1.3.11及RTX6afb9a8/标签已推送；本条文档收尾提交后继续用户组件更新需求。


### 2026-10-04 22:28 — Codex：通用组件更新识别及同版本覆盖授权

- 2026-10-04 22:28：用户“这些做完后覆盖11”明确授权同版本修订1.3.11，取代临时1.3.12开发分流，版本源已恢复1.3.11。通用Bin组件SHA256安装标记、旧安装版本待核验、同路径旧完整包/旧续传缓存失效已实现。组件状态22场景、真实下载/标记及旧缓存替换通过；0警告0错误。先前1.3.11五资产留存Artifacts/release-1.3.11-before-component，WiX已clean避免同版本载荷缓存；修订发行构建/门禁进行中，随后覆盖源码标签/GitHub/MS及本机。RTX独立包仍2026.10.04.1，不覆盖其权重/运行库。

- 修改DownloadInstallStatus/ModelDownloadPage/ModelDownloadManager：CLI列表包含sha256；组件只有核心文件+对应归档hash标记才认作最新，兼容初版1.3.11的RTX日期标记。失败/pending继续不认完成。旧组件无hash记录显示版本待核验，同路径hash改变显示可更新。下载前移除不同内容的完整旧包及旧身份续传缓存，当前身份继续断点。
- 验证release/tests/ComponentInstallStatus/Probe.vbproj：4组件各5状态+RTX两兼容状态，共22场景；真实CLI归档hash标记和旧缓存替换通过。初次在未部署aria2的build目录下载报缺依赖，复制项目内测试所需便携aria2后通过，不修改安装目录。
- 本体原版发布结果保留；本轮修订用户明确授权覆盖同版本，安装初版1.3.11的用户需安装器/ZIP手动覆盖，SemVer自动更新不触发。源码尚未提交修订。
