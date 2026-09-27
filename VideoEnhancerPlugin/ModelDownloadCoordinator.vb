Imports System
Imports System.Collections.Generic
Imports System.Threading.Tasks

Namespace videoenhancer
    Friend NotInheritable Class ModelDownloadCoordinator
        Private Const MaxParallelDownloads As Integer = 3
        Private ReadOnly _activePaths As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        Private ReadOnly _activeGroups As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

        Friend ReadOnly Property ActiveCount As Integer
            Get
                Return _activePaths.Count
            End Get
        End Property

        Friend ReadOnly Property HasCapacity As Boolean
            Get
                Return ActiveCount < MaxParallelDownloads
            End Get
        End Property

        Friend Function IsPathActive(path As String) As Boolean
            Return _activePaths.Contains(path)
        End Function

        Friend Function IsGroupActive(category As String) As Boolean
            Return _activeGroups.Contains(category)
        End Function

        Friend Function TryBeginGroup(category As String) As Boolean
            Return _activeGroups.Add(category)
        End Function

        Friend Sub EndGroup(category As String)
            _activeGroups.Remove(category)
        End Sub

        Friend Function TryBegin(path As String) As Boolean
            If Not HasCapacity Then Return False
            Return _activePaths.Add(path)
        End Function

        Friend Sub EndPath(path As String)
            _activePaths.Remove(path)
        End Sub

        Friend Async Function RunGroupAsync(Of TResult)(
            category As String, paths As List(Of String),
            start As Func(Of String, Task(Of TResult)),
            succeeded As Func(Of TResult, Boolean),
            finished As Action(Of String, TResult, Boolean),
            progress As Action(Of Integer, Boolean)) As Task(Of (Completed As Integer, Failed As Boolean))

            If Not TryBeginGroup(category) Then Return (0, False)
            Dim completed = 0
            Dim nextIndex = 0
            Dim failed = False
            Dim running As New List(Of Task(Of TResult))()
            Dim runningPaths As New Dictionary(Of Task(Of TResult), String)()
            Try
                While nextIndex < paths.Count OrElse running.Count > 0
                    While nextIndex < paths.Count AndAlso HasCapacity AndAlso Not failed
                        Dim relativePath = paths(nextIndex)
                        nextIndex += 1
                        If Not TryBegin(relativePath) Then Continue While
                        Dim task = start(relativePath)
                        running.Add(task)
                        runningPaths(task) = relativePath
                    End While
                    If running.Count = 0 Then Exit While
                    Dim completedTask = Await Task.WhenAny(running)
                    running.Remove(completedTask)
                    Dim completedPath = runningPaths(completedTask)
                    runningPaths.Remove(completedTask)
                    Dim result = Await completedTask
                    If succeeded(result) Then
                        completed += 1
                    Else
                        failed = True
                    End If
                    finished(completedPath, result, failed)
                    progress(completed, failed)
                End While
            Finally
                EndGroup(category)
            End Try
            Return (completed, failed)
        End Function
    End Class
End Namespace
