Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Diagnostics
Imports System.Text
Imports System.Text.Json

Namespace videoenhancer
    Friend NotInheritable Class ModelCatalogClient
        Private Shared Function ReadCatalogOutput(child As Process, timeoutMs As Integer) As String
            ' 必须先并行排空管道，再等待退出，否则大量诊断输出会阻塞目录加载。
            Dim output = child.StandardOutput.ReadToEndAsync()
            Dim errors = child.StandardError.ReadToEndAsync()
            If Not child.WaitForExit(timeoutMs) Then
                child.Kill(True)
                Throw New TimeoutException("模型目录查询超时")
            End If
            Dim stderr = errors.GetAwaiter().GetResult()
            Dim stdout = output.GetAwaiter().GetResult()
            If child.ExitCode <> 0 Then Throw New InvalidOperationException(PluginPanel.LastNonEmptyLine(stderr))
            Return stdout
        End Function

        Friend Shared Function RunListModels(exePath As String, ParamArray extraArgs As String()) As List(Of String)
            Dim models As New List(Of String)
            Try
                Dim psi As New ProcessStartInfo With {
                    .FileName = exePath,
                    .UseShellExecute = False,
                    .RedirectStandardOutput = True,
                    .RedirectStandardError = True,
                    .CreateNoWindow = True,
                    .StandardOutputEncoding = Encoding.UTF8
                }
                PortableRuntime.ConfigureProcess(psi)
                psi.ArgumentList.Add("--json")
                For Each a In extraArgs
                    If Not String.IsNullOrWhiteSpace(a) Then
                        psi.ArgumentList.Add(a)
                    End If
                Next
                Using p = Process.Start(psi)
                    If p Is Nothing Then
                        Return models
                    End If
                    Dim stdout = ReadCatalogOutput(p, 60000)
                    Dim firstLine = stdout.Split(Convert.ToChar(10)).FirstOrDefault(Function(l) l.Trim().StartsWith("["c))
                    If Not String.IsNullOrWhiteSpace(firstLine) Then
                        Try
                            Dim parsed = JsonSerializer.Deserialize(Of List(Of String))(firstLine.Trim())
                            If parsed IsNot Nothing Then
                                For Each modelName In parsed
                                    If Not String.IsNullOrWhiteSpace(modelName) Then
                                        models.Add(modelName.Trim())
                                    End If
                                Next
                                Return models.Distinct(StringComparer.OrdinalIgnoreCase).ToList()
                            End If
                        Catch
                            models.Clear()
                        End Try
                    End If
                    If models.Count = 0 Then
                        For Each line As String In stdout.Split(Convert.ToChar(10))
                            Dim trimmed = line.Trim()
                            If trimmed = "" OrElse trimmed.StartsWith("("c) OrElse trimmed.Contains("：") Then
                                Continue For
                            End If
                            Dim modelName = trimmed
                            Dim paren = trimmed.IndexOf("  (", StringComparison.Ordinal)
                            If paren > 0 Then
                                modelName = trimmed.Substring(0, paren).Trim()
                            End If
                            If modelName.Length > 0 AndAlso Not modelName.Contains(" "c) Then
                                models.Add(modelName)
                            End If
                        Next
                    End If
                End Using
            Catch
            End Try
            Return models.Distinct(StringComparer.OrdinalIgnoreCase).ToList()
        End Function

        Friend Shared Function RunModelCatalog(exePath As String, ParamArray extraArgs As String()) As List(Of ModelCatalogItem)
            Dim models As New List(Of ModelCatalogItem)()
            Try
                Dim psi As New ProcessStartInfo With {
                    .FileName = exePath,
                    .UseShellExecute = False,
                    .RedirectStandardOutput = True,
                    .RedirectStandardError = True,
                    .CreateNoWindow = True,
                    .StandardOutputEncoding = Encoding.UTF8,
                    .StandardErrorEncoding = Encoding.UTF8
                }
                PortableRuntime.ConfigureProcess(psi)
                psi.ArgumentList.Add("--json")
                For Each argument In extraArgs
                    If Not String.IsNullOrWhiteSpace(argument) Then psi.ArgumentList.Add(argument)
                Next
                Using child = Diagnostics.Process.Start(psi)
                    If child Is Nothing Then Return models
                    Dim stdout = ReadCatalogOutput(child, 180000)
                    Dim jsonLine = stdout.Replace(Convert.ToChar(13).ToString(), "").
                        Split(New Char() {Convert.ToChar(10)}, StringSplitOptions.RemoveEmptyEntries).
                        LastOrDefault(Function(line) line.Trim().StartsWith("["c))
                    If String.IsNullOrWhiteSpace(jsonLine) Then Return models
                    Dim parsed = JsonSerializer.Deserialize(Of List(Of ModelCatalogItem))(jsonLine.Trim(),
                        New JsonSerializerOptions With {.PropertyNameCaseInsensitive = True})
                    If parsed IsNot Nothing Then
                        models.AddRange(parsed.Where(Function(item) item IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(item.Id)))
                    End If
                End Using
            Catch
                models.Clear()
            End Try
            Return models.GroupBy(Function(item) item.Id, StringComparer.OrdinalIgnoreCase).
                Select(Function(group) group.First()).ToList()
        End Function

        Friend Shared Function RunUserModelList(exePath As String) As List(Of UserModelItem)
            Dim models As New List(Of UserModelItem)()
            Dim psi As New ProcessStartInfo With {
                .FileName = exePath,
                .UseShellExecute = False,
                .RedirectStandardOutput = True,
                .RedirectStandardError = True,
                .CreateNoWindow = True,
                .StandardOutputEncoding = Encoding.UTF8,
                .StandardErrorEncoding = Encoding.UTF8
            }
            PortableRuntime.ConfigureProcess(psi)
            psi.ArgumentList.Add("--json")
            psi.ArgumentList.Add("--list-user-models")
            Using child = Diagnostics.Process.Start(psi)
                If child Is Nothing Then Throw New InvalidOperationException("无法启动用户模型清单进程")
                Dim stdout = ReadCatalogOutput(child, 30000)
                Dim jsonLine = stdout.Replace(Convert.ToChar(13).ToString(), "").
                    Split(New Char() {Convert.ToChar(10)}, StringSplitOptions.RemoveEmptyEntries).
                    LastOrDefault(Function(line) line.Trim().StartsWith("["c))
                If String.IsNullOrWhiteSpace(jsonLine) Then Return models
                Dim parsed = JsonSerializer.Deserialize(Of List(Of UserModelItem))(jsonLine.Trim(),
                    New JsonSerializerOptions With {.PropertyNameCaseInsensitive = True})
                If parsed IsNot Nothing Then models.AddRange(parsed.Where(Function(item) item IsNot Nothing))
            End Using
            Return models
        End Function

    End Class
End Namespace
