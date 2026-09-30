Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Drawing
Imports System.IO
Imports System.Net.Http
Imports System.Linq
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.Json
Imports System.Text.RegularExpressions
Imports System.Reflection
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports FFmpegFreeUI
Imports LakeUI

Namespace videoenhancer

    Public Partial Class PluginPanel
        Private Const TutorialUrl As String = "https://www1.arxchem.top/docs/6-videoenhancer"
        Private Const TutorialApiUrl As String = "https://www1.arxchem.top/api/articles/doc-6-videoenhancer"
        Private Const TutorialImageBaseUrl As String = "https://www1.arxchem.top"

        Private Shared Function BeginnerTutorialMarkdown() As String
            Return "# 使用教程" & Environment.NewLine & Environment.NewLine &
                "正在加载 ARXChem 编写的教程……" & Environment.NewLine & Environment.NewLine &
                "[在浏览器打开教程](" & TutorialUrl & ")"
        End Function

        Private Async Sub LoadOnlineTutorialAsync(viewer As MarkDownViewer)
            Try
                Using client As New HttpClient()
                    client.Timeout = TimeSpan.FromSeconds(20)
                    client.DefaultRequestVersion = System.Net.HttpVersion.Version11
                    client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact
                    Dim response = Await client.GetStringAsync(TutorialApiUrl)
                    Using document = JsonDocument.Parse(response)
                        Dim markdown = document.RootElement.GetProperty("article").GetProperty("content").GetString()
                        If String.IsNullOrWhiteSpace(markdown) Then Throw New InvalidDataException("在线教程内容为空")
                        markdown = Await CacheTutorialImagesAsync(client, markdown)
                        If Me.IsDisposed OrElse viewer.IsDisposed Then Return
                        viewer.SetMarkdownImmediate("[在浏览器打开原教程](" & TutorialUrl & ")" &
                            Environment.NewLine & Environment.NewLine & markdown)
                    End Using
                End Using
            Catch
                If Me.IsDisposed OrElse viewer.IsDisposed Then Return
                viewer.SetMarkdownImmediate("# 使用教程" & Environment.NewLine & Environment.NewLine &
                    "在线教程暂时无法加载，请点击下方链接在浏览器中阅读。" & Environment.NewLine & Environment.NewLine &
                    "[打开 ARXChem 的 VideoEnhancer 教程](" & TutorialUrl & ")")
            End Try
        End Sub

        Private Shared Async Function CacheTutorialImagesAsync(client As HttpClient, markdown As String) As Task(Of String)
            Dim cacheDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                              "VideoEnhancer", "TutorialImages")
            Dim imageMatches = Regex.Matches(markdown, "!\[[^\]]*\]\((?<url>[^)]+)\)")
            Dim downloads As New Dictionary(Of String, Task(Of String))(StringComparer.OrdinalIgnoreCase)
            Using limiter As New Threading.SemaphoreSlim(4)
                For Each imageMatch As Match In imageMatches
                    Dim imageUrl = imageMatch.Groups("url").Value
                    If imageUrl.StartsWith("/media/", StringComparison.OrdinalIgnoreCase) Then
                        imageUrl = TutorialImageBaseUrl & imageUrl
                    End If
                    If imageUrl.StartsWith(TutorialImageBaseUrl & "/media/", StringComparison.OrdinalIgnoreCase) AndAlso
                       Not downloads.ContainsKey(imageUrl) Then
                        downloads(imageUrl) = DownloadTutorialImageAsync(client, imageUrl, cacheDirectory, limiter)
                    End If
                Next
                If downloads.Count > 0 Then Await Task.WhenAll(downloads.Values)

                For Each imageMatch As Match In imageMatches
                    Dim imageUrl = imageMatch.Groups("url").Value
                    If Regex.IsMatch(imageUrl, "^[A-Za-z]:[\\/]") Then
                        ' 原文中有作者电脑上的 Typora 路径，无法从网站取得这张图片。
                        markdown = markdown.Replace(imageMatch.Value, "*原教程中的这张图片未上传，暂时无法显示。*")
                        Continue For
                    End If
                    If imageUrl.StartsWith("/media/", StringComparison.OrdinalIgnoreCase) Then
                        imageUrl = TutorialImageBaseUrl & imageUrl
                    ElseIf Not imageUrl.StartsWith(TutorialImageBaseUrl & "/media/", StringComparison.OrdinalIgnoreCase) Then
                        Continue For
                    End If
                    Dim imagePath = Await downloads(imageUrl)
                    If imagePath Is Nothing Then
                        ' 单张图片下载失败时保留可点击的原图地址，不影响其余教程内容。
                        markdown = markdown.Replace(imageMatch.Value, "[图片暂时无法加载，点击查看原图](" & imageUrl & ")")
                    Else
                        markdown = markdown.Replace(imageMatch.Groups("url").Value, imagePath.Replace("\", "/"))
                    End If
                Next
            End Using
            Return markdown
        End Function

        Private Shared Async Function DownloadTutorialImageAsync(client As HttpClient, imageUrl As String,
                                                                  cacheDirectory As String,
                                                                  limiter As Threading.SemaphoreSlim) As Task(Of String)
            Await limiter.WaitAsync().ConfigureAwait(False)
            Try
                Dim urlHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(imageUrl)))
                Dim imagePath = Path.Combine(cacheDirectory, urlHash & "-fit960.png")
                If Not File.Exists(imagePath) Then
                    Using response = Await client.GetAsync(imageUrl).ConfigureAwait(False)
                        response.EnsureSuccessStatusCode()
                        If response.Content.Headers.ContentType?.MediaType?.StartsWith("image/", StringComparison.OrdinalIgnoreCase) <> True Then
                            Throw New InvalidDataException("教程图片响应不是图片")
                        End If
                        Dim imageBytes = Await response.Content.ReadAsByteArrayAsync().ConfigureAwait(False)
                        Directory.CreateDirectory(cacheDirectory)
                        Using imageStream As New MemoryStream(imageBytes)
                            Using sourceImage = Image.FromStream(imageStream)
                                ' 教程图片总解码量过大，滚动时会触发 LakeUI 共享缓存回收。
                                Dim targetWidth = Math.Min(sourceImage.Width, 960)
                                Dim targetHeight = Math.Max(1, CInt(Math.Round(sourceImage.Height * CDbl(targetWidth) / sourceImage.Width)))
                                Using opaqueImage As New Bitmap(targetWidth, targetHeight,
                                                                Imaging.PixelFormat.Format24bppRgb)
                                    Using canvasGraphics = Graphics.FromImage(opaqueImage)
                                        canvasGraphics.Clear(Color.FromArgb(32, 34, 38))
                                        canvasGraphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                                        canvasGraphics.DrawImage(sourceImage, 0, 0, targetWidth, targetHeight)
                                    End Using
                                    opaqueImage.Save(imagePath, Imaging.ImageFormat.Png)
                                End Using
                            End Using
                        End Using
                    End Using
                End If
                Return imagePath
            Catch
                Return Nothing
            Finally
                limiter.Release()
            End Try
        End Function

        Private Sub BuildMarkdownPage(page As ModernPanel, markdown As String)
            page.Dock = DockStyle.Fill
            page.BackColor = Color.Transparent
            page.BackColor1 = Color.Transparent
            page.BackgroundSource = ModernPanel1
            page.BorderSize = 0
            page.Padding = New Padding(0, 8, 0, 0)
            _markdownSources(page) = If(markdown, "")
        End Sub

        Private Sub EnsureMarkdownPage(page As ModernPanel)
            If page Is Nothing OrElse _markdownReady.Contains(page) Then Return
            Dim markdown As String = ""
            If Not _markdownSources.TryGetValue(page, markdown) Then Return
            Dim viewer As New MarkDownViewer With {
                .Dock = DockStyle.Fill,
                .Margin = Padding.Empty,
                .Padding = New Padding(10, 8, 10, 12),
                .BackColor = Color.Transparent,
                .BackgroundSource = ModernPanel1,
                .BorderSize = 0
            }
            viewer.ScrollBarWidth = 10
            viewer.ScrollBarTrackColor = Color.FromArgb(18, 18, 18)
            viewer.ScrollBarColor = Color.FromArgb(72, 72, 72)
            viewer.ScrollBarHoverColor = Color.FromArgb(104, 104, 104)
            viewer.HeadingColor = UiText
            viewer.BoldColor = UiText
            viewer.LinkColor = UiAccent
            viewer.CodeBackColor = Color.FromArgb(44, 44, 48)
            viewer.CodeBlockBackColor = Color.FromArgb(32, 34, 38)
            viewer.CodeBlockForeColor = UiTextSecondary
            AddHandler viewer.LinkClicked,
                Sub(sender, args)
                    Try
                        If args Is Nothing OrElse String.IsNullOrWhiteSpace(args.LinkText) Then Return
                        Process.Start(New ProcessStartInfo With {
                            .FileName = args.LinkText,
                            .UseShellExecute = True})
                    Catch
                        ' 外部链接无法打开时不影响教程页面和插件主流程。
                    End Try
                End Sub
            viewer.SetMarkdownImmediate(markdown)
            page.Controls.Add(viewer)
            _markdownReady.Add(page)
            If page Is _pageTutorial Then LoadOnlineTutorialAsync(viewer)
        End Sub
    End Class

End Namespace
