Imports System.IO
Imports videoenhancer

Module Program
    Sub Main()
        Dim core = Path.GetFullPath("Artifacts/component-install-status/" & Guid.NewGuid().ToString("N"))
        Dim paths = {"Bin/ffmpeg.7z", "Bin/mkvtoolnix.7z", "Bin/PortableGit.7z", "Bin/rtx-video/RTXVideoRuntime_20261004.7z"}
        Try
            For Each relativePath In paths
                Dim executable = DownloadInstallStatus.ComponentCoreFile(core, relativePath)
                Directory.CreateDirectory(Path.GetDirectoryName(executable))
                File.WriteAllText(executable, "fixture")
                AssertInstalled(relativePath, core, "new-hash", False, "旧安装无记录")
                Dim marker = DownloadInstallStatus.ComponentArchiveMarkerPath(core, relativePath)
                Directory.CreateDirectory(Path.GetDirectoryName(marker))
                File.WriteAllText(marker, "old-hash")
                AssertInstalled(relativePath, core, "new-hash", False, "同路径包已更新")
                File.WriteAllText(marker, "new-hash")
                AssertInstalled(relativePath, core, "NEW-HASH", True, "已安装当前内容")
                Dim archive = Path.Combine(core, relativePath.Replace("Bin/", "bin/"))
                Dim pending = archive & ".pending"
                Directory.CreateDirectory(Path.GetDirectoryName(pending))
                File.WriteAllText(pending, "fixture")
                AssertInstalled(relativePath, core, "new-hash", False, "安装中断")
                File.Delete(pending)
                File.Delete(executable)
                AssertInstalled(relativePath, core, "new-hash", False, "组件被移除")
            Next
            Dim rtx = paths(3)
            Dim rtxFile = DownloadInstallStatus.ComponentCoreFile(core, rtx)
            File.WriteAllText(rtxFile, "fixture")
            File.Delete(DownloadInstallStatus.ComponentArchiveMarkerPath(core, rtx))
            File.WriteAllText(Path.Combine(core, "bin", "rtx-video", ".installed-version"), rtx)
            AssertInstalled(rtx, core, "new-hash", False, "仅日期记录不能证明当前包内容")
            AssertInstalled(rtx, core, "", True, "兼容未返回哈希的旧CLI")
            AssertInstalled("Bin/rtx-video/RTXVideoRuntime_20261005.7z", core, "next-hash", False, "RTX日期更新")
            Console.WriteLine("COMPONENT_INSTALL_STATUS_PASS|23-scenarios|4-components|legacy-date-without-hash|same-date-content-change|RTX-new-date")
        Finally
            If Directory.Exists(core) Then Directory.Delete(core, True)
        End Try
    End Sub

    Private Sub AssertInstalled(relativePath As String, core As String, hash As String, expected As Boolean, scenario As String)
        If DownloadInstallStatus.IsDownloadInstalled(relativePath, core, "", hash) <> expected Then
            Throw New Exception(relativePath & "：" & scenario)
        End If
    End Sub
End Module
