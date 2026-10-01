namespace VideoEnhancer;

/// <summary>由插件在便携工作目录写入取消标记；安装事务开始后不再中断。</summary>
internal static class DownloadCancellation
{
    private static readonly CancellationTokenSource Source = new();
    private static readonly string? Marker = Environment.GetEnvironmentVariable("VIDEOENHANCER_CANCEL_FILE");
    private static readonly Timer Watcher = new(_ =>
    {
        if (Marker is not null && File.Exists(Marker)) Source.Cancel();
    }, null, Marker is null ? Timeout.Infinite : 0, Marker is null ? Timeout.Infinite : 100);

    internal static CancellationToken Token
    {
        get
        {
            if (Marker is not null && File.Exists(Marker)) Source.Cancel();
            return Source.Token;
        }
    }

    internal static void Check() => Source.Token.ThrowIfCancellationRequested();

    internal static void Log(string operation, Exception error)
    {
        try
        {
            var folder = Path.Combine(PortablePaths.CoreRoot, "logs");
            Directory.CreateDirectory(folder);
            File.AppendAllText(Path.Combine(folder, "downloads.log"),
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {operation}{Environment.NewLine}{error}{Environment.NewLine}",
                new System.Text.UTF8Encoding(false));
        }
        catch (Exception logError) { System.Diagnostics.Trace.WriteLine(logError); }
    }
}
