Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks
Imports videoenhancer

Module Program
    Private Sub Check(condition As Boolean, message As String)
        If Not condition Then Throw New Exception(message)
    End Sub

    Private Sub Scenario(first As ModelDownloadCoordinator.ItemOutcome,
                         expectedStop As ModelDownloadCoordinator.QueueStopReason,
                         Optional stopByUser As Boolean = False,
                         Optional completedFatal As Boolean = False)
        Dim coordinator As New ModelDownloadCoordinator()
        Dim paths = Enumerable.Range(1, 7).Select(Function(n) n.ToString()).ToList()
        Dim tasks As New Dictionary(Of String, TaskCompletionSource(Of ModelDownloadCoordinator.ItemOutcome))()
        Dim started As New List(Of String)()
        Dim stopping = False
        Dim maximum = 0
        Dim states As New List(Of String)()
        Dim batchTask = coordinator.RunGroupAsync("test", paths,
            Function(path)
                Check(Not tasks.ContainsKey(path), "本轮重复启动文件")
                started.Add(path)
                Dim source As New TaskCompletionSource(Of ModelDownloadCoordinator.ItemOutcome)()
                tasks.Add(path, source)
                maximum = Math.Max(maximum, coordinator.ActiveCount)
                Return source.Task
            End Function,
            Function(result) result,
            Sub(path, result)
                ' 模拟另一个任务在补位前已返回致命错误。
                If completedFatal AndAlso path = "1" Then tasks("2").SetResult(ModelDownloadCoordinator.ItemOutcome.Offline)
            End Sub,
            Sub(state) states.Add(state.Summary),
            Function() stopping)
        Check(started.SequenceEqual({"1", "2", "3"}), "初始窗口不是三个任务")
        stopping = stopByUser
        tasks("1").SetResult(first)
        Dim shouldContinue = expectedStop = ModelDownloadCoordinator.QueueStopReason.None
        Check(started.Count = If(shouldContinue, 4, 3), "首项结束后的补位或停止不正确")
        Dim index = 1
        While index < started.Count
            Dim source = tasks(started(index))
            If Not source.Task.IsCompleted Then source.SetResult(ModelDownloadCoordinator.ItemOutcome.Succeeded)
            index += 1
        End While
        Dim batch = batchTask.GetAwaiter().GetResult()
        Check(maximum = 3 AndAlso coordinator.ActiveCount = 0, "并发上限或槽位释放错误")
        Check(batch.StopReason = expectedStop, "停止原因错误")
        Check(batch.Pending = If(shouldContinue, 0, 4) AndAlso batch.Running = 0, "剩余数量错误")
        Check(batch.Succeeded + batch.Cancelled + batch.Failed + batch.Pending = 7, "分类统计不守恒")
        If first = ModelDownloadCoordinator.ItemOutcome.Cancelled Then Check(batch.Cancelled = 1, "取消被算作失败")
        If first = ModelDownloadCoordinator.ItemOutcome.Failed Then Check(batch.Failed = 1, "普通失败统计错误")
        If shouldContinue Then Check(started.SequenceEqual(paths), "队列未按原顺序全部启动")
        Check(Not coordinator.IsGroupActive("test"), "队列状态未释放")
        Console.WriteLine($"PASS {first}/{expectedStop}/simultaneous={completedFatal}")
    End Sub

    Private Sub StopLastTask()
        Dim coordinator As New ModelDownloadCoordinator()
        Dim tasks As New List(Of TaskCompletionSource(Of ModelDownloadCoordinator.ItemOutcome))()
        Dim stopping = False
        Dim batchTask = coordinator.RunGroupAsync("last", New List(Of String) From {"1", "2", "3"},
            Function(path)
                Dim source As New TaskCompletionSource(Of ModelDownloadCoordinator.ItemOutcome)()
                tasks.Add(source)
                Return source.Task
            End Function, Function(result) result, Sub(path, result)
                                                  End Sub, Sub(state)
                                                             End Sub, Function() stopping)
        tasks(0).SetResult(ModelDownloadCoordinator.ItemOutcome.Succeeded)
        tasks(1).SetResult(ModelDownloadCoordinator.ItemOutcome.Succeeded)
        stopping = True
        tasks(2).SetResult(ModelDownloadCoordinator.ItemOutcome.Cancelled)
        Check(batchTask.GetAwaiter().GetResult().StopReason = ModelDownloadCoordinator.QueueStopReason.UserStopped, "最后一个任务取消丢失停止原因")
        Console.WriteLine("PASS stop last active task")
    End Sub

    Private Sub RetryQueue(first As ModelDownloadCoordinator.ItemOutcome)
        Dim coordinator As New ModelDownloadCoordinator()
        Dim started As New List(Of String)()
        Dim tasks As New Dictionary(Of String, TaskCompletionSource(Of ModelDownloadCoordinator.ItemOutcome))()
        Dim batchTask = coordinator.RunGroupAsync("全部资源", New List(Of String) From {"1", "2", "3", "4", "5"},
            Function(path)
                started.Add(path)
                Dim source As New TaskCompletionSource(Of ModelDownloadCoordinator.ItemOutcome)()
                tasks(path) = source
                Check(coordinator.ActiveCount <= 3, "重试超过并发上限")
                Return source.Task
            End Function, Function(result) result, Sub(path, result)
                                                  End Sub, Sub(state)
                                                             End Sub, Function() False)
        tasks("1").SetResult(first)
        Check(coordinator.Enqueue("PTH", "1") = ModelDownloadCoordinator.EnqueueResult.Enqueued, "重试未加入全部队列")
        Check(coordinator.Enqueue("PTH", "1") = ModelDownloadCoordinator.EnqueueResult.AlreadyQueued, "重复点击创建重复队列项")
        tasks("2").SetResult(ModelDownloadCoordinator.ItemOutcome.Succeeded)
        tasks("3").SetResult(ModelDownloadCoordinator.ItemOutcome.Succeeded)
        Check(started.SequenceEqual({"1", "2", "3", "4", "5", "1"}), "重试没有排到队列尾部")
        For Each path In {"4", "5", "1"}
            tasks(path).SetResult(ModelDownloadCoordinator.ItemOutcome.Succeeded)
        Next
        Dim batch = batchTask.GetAwaiter().GetResult()
        Check(batch.Succeeded = 5 AndAlso batch.Cancelled + batch.Failed = 0 AndAlso batch.Pending = 0, "重试计数错误")
        Check(coordinator.Enqueue("PTH", "1") = ModelDownloadCoordinator.EnqueueResult.NoQueue, "队列结束后未释放状态")
        Console.WriteLine("PASS explicit retry at tail: " & first.ToString())
    End Sub

    Private Sub RetryIdleSlot()
        Dim coordinator As New ModelDownloadCoordinator()
        Dim tasks As New List(Of TaskCompletionSource(Of ModelDownloadCoordinator.ItemOutcome))()
        Dim stopped = False
        Dim batchTask = coordinator.RunGroupAsync("PTH", New List(Of String) From {"1", "2"},
            Function(path)
                Dim source As New TaskCompletionSource(Of ModelDownloadCoordinator.ItemOutcome)()
                tasks.Add(source)
                Return source.Task
            End Function, Function(result) result, Sub(path, result)
                                                  End Sub, Sub(state)
                                                             End Sub, Function() stopped)
        Check(coordinator.Enqueue("PTH", "3") = ModelDownloadCoordinator.EnqueueResult.Enqueued AndAlso tasks.Count = 3, "空闲槽位未立即启动新项")
        stopped = True
        Check(coordinator.Enqueue("PTH", "4") = ModelDownloadCoordinator.EnqueueResult.Stopping, "停止后仍接受重试")
        For Each source In tasks
            source.SetResult(ModelDownloadCoordinator.ItemOutcome.Succeeded)
        Next
        Check(batchTask.GetAwaiter().GetResult().StopReason = ModelDownloadCoordinator.QueueStopReason.UserStopped, "停止原因错误")
        Console.WriteLine("PASS enqueue wakes idle slot and rejects stopping queue")
    End Sub

    Sub Main()
        Scenario(ModelDownloadCoordinator.ItemOutcome.Succeeded, ModelDownloadCoordinator.QueueStopReason.None)
        Scenario(ModelDownloadCoordinator.ItemOutcome.Cancelled, ModelDownloadCoordinator.QueueStopReason.None)
        Scenario(ModelDownloadCoordinator.ItemOutcome.Failed, ModelDownloadCoordinator.QueueStopReason.None)
        Scenario(ModelDownloadCoordinator.ItemOutcome.Offline, ModelDownloadCoordinator.QueueStopReason.Offline)
        Scenario(ModelDownloadCoordinator.ItemOutcome.AuthenticationRequired, ModelDownloadCoordinator.QueueStopReason.AuthenticationRequired)
        Scenario(ModelDownloadCoordinator.ItemOutcome.Cancelled, ModelDownloadCoordinator.QueueStopReason.UserStopped, stopByUser:=True)
        Scenario(ModelDownloadCoordinator.ItemOutcome.Succeeded, ModelDownloadCoordinator.QueueStopReason.Offline, completedFatal:=True)
        StopLastTask()
        RetryQueue(ModelDownloadCoordinator.ItemOutcome.Cancelled)
        RetryQueue(ModelDownloadCoordinator.ItemOutcome.Failed)
        RetryIdleSlot()
        Console.WriteLine("DOWNLOAD_QUEUE_TESTS_PASS|11")
    End Sub
End Module
