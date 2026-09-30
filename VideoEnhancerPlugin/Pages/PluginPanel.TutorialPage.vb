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

        Private Shared Function BeginnerTutorialMarkdown() As String
            Return "# 使用教程" & Environment.NewLine & Environment.NewLine &
                "正在加载 ARXChem 编写的教程……" & Environment.NewLine & Environment.NewLine &
                "[在浏览器打开教程](" & TutorialUrl & ")"
        End Function

        Private Async Sub LoadOnlineTutorialAsync(viewer As MarkDownViewer)
            Try
                Using client As New HttpClient()
                    client.Timeout = TimeSpan.FromSeconds(20)
                    Dim response = Await client.GetStringAsync(TutorialApiUrl)
                    Using document = JsonDocument.Parse(response)
                        Dim markdown = document.RootElement.GetProperty("article").GetProperty("content").GetString()
                        If String.IsNullOrWhiteSpace(markdown) Then Throw New InvalidDataException("在线教程内容为空")
                        ' 站点文章中的图片使用根路径，补全地址后交给 LakeUI Markdown 阅读器。
                        markdown = markdown.Replace("](/media/", "](https://www1.arxchem.top/media/")
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
