Imports System
Imports System.Collections.Generic

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
    End Class
End Namespace
