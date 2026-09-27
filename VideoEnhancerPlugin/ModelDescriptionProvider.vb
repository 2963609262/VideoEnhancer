Imports System
Imports System.Collections.Generic
Imports System.Linq

Namespace videoenhancer
    Friend NotInheritable Class ModelDescriptionProvider
        Friend Shared Function FallbackArchitecture(modelId As String) As String
            Dim normalized = If(modelId, "").Replace(Convert.ToChar(92), "/"c)
            For Each architecture In New String() {
                "RealESRGAN", "RealHatGAN", "ESRGAN", "SPANPlus", "SPAN", "SwinIR", "RealCUGAN",
                "AnimeSR", "CRAFT", "DITN", "MoSR", "RIFE", "GMFSS", "GIMM"
            }
                If normalized.IndexOf(architecture, StringComparison.OrdinalIgnoreCase) >= 0 Then Return architecture
            Next
            Dim segments = normalized.Split(New Char() {"/"c}, StringSplitOptions.RemoveEmptyEntries)
            Return If(segments.Length > 1, segments(0), "其他模型")
        End Function

        Friend Shared Function ModelIntroduction(entry As ModelCatalogItem,
                                                   interpolation As Boolean) As String
            If entry Is Nothing Then Return ""
            If interpolation Then Return InterpolationModelIntroduction(entry)

            Dim modelId = If(entry.Id, "").Replace(Convert.ToChar(92), "/").Trim()
            Dim displayName = If(entry.DisplayName, "").Trim()
            Dim key = (modelId & " " & displayName).ToLowerInvariant()
            Dim architecture = If(entry.Architecture, "").Trim().ToUpperInvariant()

            If key.Contains("basicvsr") Then
                Return "BasicVSR++ REDS4：利用相邻视频帧做时序复原，适合画面连续的低清视频；它不是普通单帧放大，当前不能再叠加运动补帧。"
            End If
            If key.Contains("flashvsr") Then
                Return "FlashVSR：面向连续视频的时序超分模型，适合希望一次处理运动连续性与分辨率的 NVIDIA 用户；它不是普通图片模型，当前不参与通用补帧组合。"
            End If

            If key.Contains("animejanai-hd-v3.1") Then
                Dim preset = If(key.Contains("sharp1") AndAlso key.Contains("performance"),
                    "Sharp1 Performance：在较轻量的 Performance 配置上进一步强调边缘清晰度。",
                    If(key.Contains("sharp1"),
                        "Sharp1 Balanced：在 Balanced 配置上额外强调线稿和边缘。",
                        If(key.Contains("performance"),
                            "Performance：优先考虑处理速度和显存占用。",
                            "Balanced：在清晰度、稳定性和资源占用之间取平衡。")))
                Return "AnimeJaNai HD V3.1 " & preset & " 这是 2x 动漫/插画模型；干净的高清原片可优先用 Sharp1，普通素材先用 Balanced，显存紧张时选 Performance。"
            End If
            If key.Contains("animejanai-sd-v1beta34") Then
                Return "AnimeJaNai SD V1 beta34 Compact strong：针对较低清晰度动漫素材做 2x 强增强；低清噪点和压缩块也可能被放大，更适合噪声较少的动漫素材。"
            End If
            If key.Contains("animejanai-v3") Then
                Return "AnimeJaNai V3 HD Sharp1 Compact：2x 动漫/插画模型，Compact 版本较易运行，Sharp1 会更强调线稿边缘；原片已有噪点时先比较是否过锐。"
            End If
            If key.Contains("animejanai-v2") Then
                Return "AnimeJaNai V2 Compact：2x 动漫/插画轻量模型，适合第一次测试、预览或显存较紧张的设备；想要更强边缘强调可改试 V3 Sharp1。"
            End If

            If key.Contains("anisd") Then
                Return AniSdModelIntroduction(key, architecture)
            End If

            If key.Contains("realhatgan") Then
                If key.Contains("x1") OrElse key.Contains("fix-only") Then
                    Return "RealHatGAN JP Illustration x1 修复版：只修复插画纹理和边缘，不改变分辨率；适合尺寸已经够大、只想减少瑕疵的素材，输入边长需按 16 的倍数处理。"
                End If
                If key.Contains("universal") Then
                    Return "RealHatGAN Universal Illustration 2x：面向不同风格插画的 2x 放大，适合不确定具体画风的二次元素材；输入边长需按 16 的倍数处理。"
                End If
                If key.Contains("4x") Then
                    Return "RealHatGAN JP Illustration 4x：针对日式插画做 4x 放大，适合需要大幅放大的线稿和绘画素材；输入边长需按 16 的倍数处理，显存压力也更高。"
                End If
                Return "RealHatGAN JP Illustration 2x：针对日式插画做中等幅度放大，适合先保留线稿结构再增加纹理；输入边长需按 16 的倍数处理。"
            End If

            If key.Contains("animevideov3") Then
                If key.Contains("-2x") Then
                    Return "Real-ESRGAN AnimeVideoV3 2x：为动漫视频准备的 2x 模型，适合原片尚清楚、只需要温和放大的情况，速度和细节风险都较易控制。"
                End If
                If key.Contains("-3x") Then
                    Return "Real-ESRGAN AnimeVideoV3 3x：为动漫视频准备的 3x 模型，适合 2x 不够、4x 又过大的中间需求；适合需要中等放大幅度的动漫视频。"
                End If
                Return "Real-ESRGAN AnimeVideoV3 4x：为动漫视频准备的 4x 模型，适合低分辨率动画需要明显放大的情况；输出像素量约为 2x 的四倍，处理更慢。"
            End If
            If key.Contains("general-x4v3") Then
                Return "Real-ESRGAN General x4v3：面向真人、风景和普通网络视频的通用 4x 方案；内容类型不特殊时可优先选择，适合做通用画面放大。"
            End If
            If key.Contains("x4plus-anime") Then
                Return "Real-ESRGAN x4plus Anime：动漫/插画 4x 模型，适合线稿、平涂和角色画面；如果原片压缩严重，先用较低倍率或去噪模型比较。"
            End If
            If key.Contains("x4-jp-illustration-fix1") Then
                Return "Real-ESRGAN JP Illustration fix1：日式插画专用 4x ONNX 导出修正版 1，题材和倍率固定；它与 fix2 是不同导出版本，优先保留能在当前环境预检和运行的一份。"
            End If
            If key.Contains("x4-jp-illustration-fix2") Then
                Return "Real-ESRGAN JP Illustration fix2：日式插画专用 4x ONNX 导出修正版 2，题材和倍率固定；若 fix1 在你的 ONNX 环境异常，可用它做替代测试。"
            End If

            If key.Contains("waifu2x") Then
                If key.Contains("photo") Then
                    Return "Waifu2x Photo 2x：为照片类素材准备的 2x 模型；人物、实拍和纹理照片可先选它，纯动漫线稿优先考虑普通或 Noise 版本。"
                End If
                If key.Contains("noise3") Then
                    Return "Waifu2x Noise3 2x：2x 放大并使用最强一级去噪，适合噪声很重的动漫截图；细线和小字可能被抹掉，建议与 Noise2 对比。"
                End If
                If key.Contains("noise2") Then
                    Return "Waifu2x Noise2 2x：2x 放大并使用中等去噪，适合有明显压缩噪点但仍要保留线稿的动漫素材。"
                End If
                If key.Contains("noise1") Then
                    Return "Waifu2x Noise1 2x：2x 放大并使用轻度去噪，适合轻微噪点的动漫画面；比 Noise2 更容易保留细节。"
                End If
                If key.Contains("noise0") Then
                    Return "Waifu2x Noise0 2x：2x 放大但不主动加强去噪，适合原片干净、希望尽量保留原有纹理的动漫素材。"
                End If
                Return "Waifu2x 2x：动漫和插画的基础 2x 放大方案；素材有噪点时再按噪声强度选择 Noise1/2/3，避免一开始就过度去噪。"
            End If
            If key.Contains("cugan-conservative") Then
                Return "Real-CUGAN Conservative 2x：动漫画面的保守型 2x 放大，倾向少改动画面；适合不想出现过度锐化或新纹理的干净素材。"
            End If
            If key.Contains("denoiseh264") Then
                Return "DenoiseH264 SuperUltraCompact：针对 H.264 压缩噪声的 1x 处理，不改变分辨率；适合先清理块状噪声，再交给后续放大模型。"
            End If
            If key.Contains("dncnn") Then
                Return "DnCNN ColorBlind：盲去噪 1x 模型，会根据画面估计噪声强度，不改变分辨率；适合噪声来源不明的素材，但要留意细节是否被过度抹平。"
            End If
            If key.Contains("animesr") Then
                Return "AnimeSR V2：动漫视频时序超分 4x 模型，利用相邻帧帮助保持动画细节连续；当前清单仅支持 CUDA，适合 NVIDIA 用户处理动漫视频。"
            End If
            If key.Contains("apisr-dat") Then
                Return "APISR DAT GAN 4x：面向动漫/插画纹理恢复的 4x GAN 模型，适合希望补回细节的素材；GAN 可能生成看似合理的新纹理，建议先看脸部和文字。"
            End If
            If key.Contains("apisr-grl") Then
                Return "APISR GRL GAN 4x：APISR 的 GRL 4x 纹理恢复版本，适合细节丰富的动漫/插画；更适合需要明显补回纹理的画面。"
            End If
            If key.Contains("apisr-rrdb") Then
                If key.Contains("-2x") Then
                    Return "APISR RRDB GAN 2x：温和的 2x 纹理恢复，适合原片分辨率尚可、只想补一点细节的动漫/插画。"
                End If
                Return "APISR RRDB GAN 4x：需要明显放大的 4x 纹理恢复版本；比 2x 更吃资源，也更容易增强细线、文字和重复纹理。"
            End If
            If key.Contains("aniscale2-refiner") Then
                Return "AniScale2 Refiner 1x：只做细节修复、不改变分辨率；适合先清理或整理画面，再决定是否另做 2x 放大。"
            End If
            If key.Contains("aniscale2-esrgan-lite") Then
                Return "AniScale2 ESRGAN-Lite 2x：偏轻量的动漫/插画 2x 放大，适合速度优先或显存较紧张的设备。"
            End If
            If key.Contains("aniscale2-esrgan") Then
                Return "AniScale2 ESRGAN 2x：动漫/插画的常规 2x 纹理增强，适合想比轻量版获得更强细节、又不需要 4x 的素材。"
            End If
            If key.Contains("aniscale2-ditn") Then
                Return "AniScale2 DITN 2x：动漫/插画 2x 细节恢复模型，适合想保留结构、减少过度锐化的素材。"
            End If
            If key.Contains("aniscale2-omni") Then
                Return "AniScale2 Omni 2x：面向多种动漫/插画内容的均衡 2x 方案，适合不知道该选哪种专门风格时先做基准测试。"
            End If
            If key.Contains("anitoon-rplksrl") Then
                Return "AniToon RPLKSR-L 2x：AniToon 的大模型版本，偏向保留更多动漫纹理；画质优先时使用，显存和时间开销会高于 S 版。"
            End If
            If key.Contains("anitoon-rplksrs") Then
                Return "AniToon RPLKSR-S 2x：AniToon 的小模型版本，偏向速度和较低资源占用；适合预览、批量处理或显存较紧张的设备。"
            End If
            If key.Contains("anitoon-rplksr") Then
                Return "AniToon RPLKSR 2x：AniToon 的标准 2x 动漫放大方案；想在速度与细节之间取中间位置时先用它。"
            End If
            If key.Contains("nomos8k") Then
                If key.Contains("strong") Then
                    Return "Nomos8k SPAN OTF strong 4x：高强度 4x 纹理恢复，适合细节缺失明显的素材；也最容易把噪声或错误纹理一起放大。"
                End If
                If key.Contains("weak") Then
                    Return "Nomos8k SPAN OTF weak 4x：较温和的 4x 纹理恢复，适合画面本身较干净、希望少改动原貌的素材。"
                End If
                Return "Nomos8k SPAN OTF medium 4x：中等强度 4x 纹理恢复，适合在 weak 和 strong 之间取平衡；第一次使用可先从它开始。"
            End If
            If key.Contains("modernspanimation-v3") Then
                Return "ModernSpanimation V3 2x：面向动漫画面和线稿的 SPAN 2x 版本；它与 V2 是不同训练版本，适合希望使用较新训练配置的动漫素材。"
            End If
            If key.Contains("modernspanimation-v2") Then
                Return "ModernSpanimation V2 2x：面向动漫画面和线稿的 SPAN 2x 版本，适合希望使用 V2 训练配置的动漫素材。"
            End If
            If key.Contains("bhi-spanplusdynamic") Then
                Return "BHI SpanPlus Dynamic Light 2x：轻量动态输入的 SPANPlus 2x 模型，适合希望兼顾速度与线稿细节的动漫素材。"
            End If
            If key.Contains("sudo-shuffle-span") Then
                Return "Sudo-Shuffle SPAN 2x：针对插画和动漫纹理的 2x SPAN 方案，适合想保留线稿、避免过度 GAN 纹理的素材。"
            End If
            If key.Contains("openproteus") Then
                Return "OpenProteus Compact i2 2x：轻量 2x 细节恢复模型，适合普通动漫/插画素材；处理速度和较低资源占用优先时可选它。"
            End If
            If key.Contains("ani4k-compact") Then
                Return "Ani4K Compact 2x：面向动漫画面的轻量 2x 放大，适合先快速查看模型方向；如果细节不足，再与 AnimeJaNai 或 SPAN 版本比较。"
            End If

            If key.Contains("realplksr") OrElse key.Contains("rplksr") Then
                If key.Contains("-l") Then
                    Return "RealPLKSR-L 2x：较大容量的 2x 细节恢复模型，适合画面质量优先；资源紧张时改用 S 版或 Compact 版。"
                End If
                If key.Contains("-s") Then
                    Return "RealPLKSR-S 2x：较小容量的 2x 细节恢复模型，适合预览和速度优先；细节要求高时可与标准版对比。"
                End If
                If key.Contains("dynamic") Then
                    Return "RealPLKSR 动态输入 2x：适合尺寸不固定的视频帧，按输入内容动态处理；它偏向自然的边缘与纹理恢复。"
                End If
                Return "RealPLKSR 2x：动漫/插画的均衡细节恢复模型，适合想要清晰边缘但不希望使用强 GAN 风格的素材。"
            End If

            Select Case architecture
                Case "COMPACT"
                    Return "「" & displayName & "」是 Compact 轻量 2x 模型，适合预览、批量处理或显存有限的设备；先观察清晰度，再决定是否换更大模型。"
                Case "CRAFT"
                    Return "「" & displayName & "」是 CRAFT 2x 纹理恢复模型，适合动漫/插画细节；请重点检查线稿、文字和高对比边缘。"
                Case "DAT", "DAT2"
                    Return "「" & displayName & "」是 DAT 纹理恢复模型，适合细节丰富的动漫/插画；纹理恢复取向较积极。"
                Case "DITN"
                    Return "「" & displayName & "」是 DITN 2x 细节恢复模型，适合希望增强纹理但保留原结构的动漫/插画。"
                Case "ESRGAN", "ESRGAN-LITE"
                    Return "「" & displayName & "」是动漫/插画 2x 纹理增强模型；Lite 侧重轻量，普通版侧重更充分的细节恢复。"
                Case "ESRGAN-REFINER"
                    Return "「" & displayName & "」是 1x 细节修复模型，只修画面不改分辨率；适合把去噪/修复作为独立第一步。"
                Case "GRL"
                    Return "「" & displayName & "」是 GRL 纹理恢复模型，适合细节丰富、需要补回纹理的动漫/插画。"
                Case "OMNISR"
                    Return "「" & displayName & "」是均衡型 2x 细节恢复模型，适合不同内容混合的视频，第一次选择可用它做基准。"
                Case "REAL-CUGAN"
                    Return "「" & displayName & "」是偏保守的动漫 2x 模型，适合希望少改动原画、降低过度锐化风险的素材。"
                Case "RRDBNET"
                    Return "「" & displayName & "」是 RRDB 纹理恢复模型，适合普通动漫/插画放大；高倍率更适合确实需要大幅放大的素材。"
                Case "SPAN", "SPANF3", "SPANPLUS"
                    Return "「" & displayName & "」是 SPAN 结构与纹理恢复模型，适合线稿、平涂和动漫画面；它更强调边缘，压缩噪声严重时先做去噪测试。"
                Case "SWINIR"
                    Return "「" & displayName & "」是 SwinIR 细节恢复模型，适合希望结果较稳、不过分制造纹理的动漫/插画；ONNX 固定窗口版本会自动按窗口处理。"
                Case Else
                    Return "「" & displayName & "」当前标记为 " & If(String.IsNullOrWhiteSpace(architecture), "未知架构", architecture) & "；请根据素材题材、目标倍率和可用后端选择，适合先从默认参数开始。"
            End Select
        End Function

        Friend Shared Function InterpolationModelIntroduction(entry As ModelCatalogItem) As String
            Dim modelId = If(entry.Id, "").Replace(Convert.ToChar(92), "/").Trim()
            Dim displayName = If(entry.DisplayName, "").Trim()
            Dim key = (modelId & " " & displayName).ToLowerInvariant()
            If key.Contains("rife") Then
                If key.Contains("heavy") Then
                    Return "RIFE heavy：更重的通用光流补帧模型，复杂运动时可获得更充分的运动估计；速度和显存开销较高，适合显存充足且运动复杂的素材。"
                End If
                If key.Contains("lite") Then
                    Return "RIFE lite：偏轻量的通用光流补帧模型，适合预览、批量处理或显存紧张的设备；复杂运动的余量小于 heavy。"
                End If
                If key.Contains("4.26") Then
                    Return "RIFE v4.26：通用光流补帧模型，适合真人、动画和普通镜头；它是一次稳妥的默认起点，倍率先从 2 倍开始。"
                End If
                If key.Contains("4.25") Then
                    Return "RIFE v4.25：通用光流补帧模型，适合大多数连续运动画面；快速运动或复杂遮挡的素材也可优先考虑。"
                End If
                Return "RIFE：通用光流补帧模型，给连续视频生成中间帧；适合真人、动漫和普通镜头，倍率通常从 2 倍开始。"
            End If
            If key.Contains("gmfss") Then
                If key.Contains("anime") OrElse key.Contains("animerun") Then
                    Return "GMFSS AnimeRun：针对动漫运动和线稿连续性的补帧模型，适合动画素材；真人视频请优先用 RIFE 或 GMFSS Base 做比较。"
                End If
                If key.Contains("union") Then
                    Return "GMFSS Union：通用时序补帧模型，利用更多帧信息处理复杂运动；适合想在快速镜头中提升稳定性的 NVIDIA 用户。"
                End If
                Return "GMFSS Base：通用时序补帧模型，适合真人和普通连续运动；倍率从 2 倍开始更易控制计算量。"
            End If
            If key.Contains("gimm") Then
                If key.Contains("lpips") Then
                    Return "GIMM LPIPS：强调感知相似度的时序补帧模型，适合更在意运动观感的连续视频。"
                End If
                If key.Contains("-r") Then
                    Return "GIMM R：时序补帧模型的 R 配置，适合希望保持运动结构连续的素材；适合连续性要求较高的画面。"
                End If
                If key.Contains("-f") Then
                    Return "GIMM F：时序补帧模型的 F 配置，适合希望改善运动流畅度的素材；倍率可从 2 倍、转场阈值 4.0 开始。"
                End If
                Return "GIMM：时序补帧模型，适合连续运动视频；它需要 CUDA/PyTorch，适合 NVIDIA 用户处理连续运动画面。"
            End If
            Return "「" & displayName & "」是补帧模型，用于根据相邻帧生成中间帧；倍率通常从 2 倍开始，素材运动复杂时再提高倍率。"
        End Function

        Friend Shared Function AniSdModelIntroduction(key As String, architecture As String) As String
            Dim variantName As String
            If key.Contains("ac-g6i2a") Then
                variantName = "AC-G6i2a"
            ElseIf key.Contains("ac-g6i2b") Then
                variantName = "AC-G6i2b"
            ElseIf key.Contains("dc") Then
                variantName = "DC"
            ElseIf key.Contains("db-i2") Then
                variantName = "DB-i2"
            ElseIf key.Contains("g6i1b") Then
                variantName = "G6i1b"
            ElseIf key.Contains("g6i1") Then
                variantName = "G6i1"
            ElseIf key.Contains("ps-g6i2") Then
                variantName = "PS-G6i2"
            ElseIf key.Contains("ac-") Then
                variantName = "AC"
            Else
                variantName = "AniSD"
            End If

            Dim role As String
            Select Case architecture
                Case "COMPACT"
                    role = "Compact 轻量版，适合预览和显存有限的设备"
                Case "SPAN"
                    role = "SPAN 版本，适合线稿、平涂和边缘细节"
                Case "SWINIR"
                    role = "SwinIR 版本，倾向稳定恢复纹理；ONNX 固定窗口版本会按窗口处理"
                Case "CRAFT"
                    role = "CRAFT 版本，适合细节丰富的动漫/插画"
                Case "DAT2"
                    role = "DAT2 版本，适合纹理复杂的动漫/插画"
                Case "REALPLKSR"
                    role = "RealPLKSR 版本，适合在边缘清晰与纹理自然之间取平衡"
                Case Else
                    role = If(String.IsNullOrWhiteSpace(architecture), "具体架构未标注", architecture & " 版本")
            End Select

            If key.Contains("-1x") Then
                Return "AniSD " & variantName & " " & role & "，这是 1x 修复而不是放大；适合先修画面、再另选 2x/4x 模型。"
            End If
            If key.Contains("dynamic") Then
                Return "AniSD " & variantName & " " & role & "，这是 2x 动态输入版本，适合尺寸不固定的视频帧；适合动漫和插画的常规放大。"
            End If
            If key.Contains("240x320") OrElse key.Contains("320x448") OrElse key.Contains("480x320") Then
                Dim windowSize = If(key.Contains("240x320"), "240x320", If(key.Contains("320x448"), "320x448", "480x320"))
                Return "AniSD " & variantName & " " & role & "，这是 2x ONNX 固定窗口 " & windowSize & " 版本；适合与对应窗口布局配合，程序会自动按窗口处理。"
            End If
            Return "AniSD " & variantName & " " & role & "，这是 2x 动漫/插画模型；AC、DC、DB、PS 和 G6i 代表不同训练配置，不是简单的高低档位，应按具体架构和素材特点选择。"
        End Function

        Friend Shared Function ModelTooltipText(entry As ModelCatalogItem,
                                                  interpolation As Boolean) As String
            If entry Is Nothing Then Return ""
            Dim lines As New List(Of String)()
            If String.Equals(entry.Source, "builtin", StringComparison.OrdinalIgnoreCase) Then
                lines.Add("内置模型")
            ElseIf String.Equals(entry.Source, "user", StringComparison.OrdinalIgnoreCase) Then
                lines.Add("用户导入模型")
            End If
            If Not String.IsNullOrWhiteSpace(entry.DisplayName) Then
                lines.Add("模型：" & entry.DisplayName)
            End If
            lines.Add(ModelIntroduction(entry, interpolation))
            If Not interpolation AndAlso entry.Scale > 0 Then
                lines.Add("倍率：" & entry.Scale.ToString() & "x")
            End If
            If entry.Backends IsNot Nothing AndAlso entry.Backends.Length > 0 Then
                lines.Add("支持后端：" & String.Join(" / ", entry.Backends.Select(Function(value) BackendDisplayName(value))))
            End If
            Return String.Join(Environment.NewLine, lines.Where(Function(line) Not String.IsNullOrWhiteSpace(line)))
        End Function

        Friend Shared Function BackendDisplayName(value As String) As String
            Select Case If(value, "").Trim().ToLowerInvariant()
                Case "ncnn"
                    Return "NCNN"
                Case "cuda"
                    Return "CUDA"
                Case "tensorrt"
                    Return "TensorRT"
                Case "onnx"
                    Return "ONNX"
                Case "flashvsr"
                    Return "FlashVSR"
                Case "basicvsrpp"
                    Return "BasicVSR++"
                Case Else
                    Return If(value, "").Trim()
            End Select
        End Function

    End Class
End Namespace
