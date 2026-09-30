param(
    [string]$ArchiveTool = (Join-Path $PSScriptRoot '..\Artifacts\videoenhancer.exe')
)

$ErrorActionPreference = 'Stop'
$root = Join-Path $PSScriptRoot ('..\Artifacts\.refactor-tmp\backend-history-test-' + [guid]::NewGuid().ToString('N'))
$base = Join-Path $root 'base\python'
$target = Join-Path $root 'target\python'
$output = Join-Path $root 'output'
$utf8NoBom = [System.Text.UTF8Encoding]::new($false)

function Write-TestText([string]$path, [string]$value) {
    New-Item -ItemType Directory -Force -Path (Split-Path -Parent $path) | Out-Null
    [System.IO.File]::WriteAllText($path, $value, $utf8NoBom)
}

foreach ($directory in @($base, $target)) {
    Write-TestText (Join-Path $directory 'python\python.exe') 'python'
    Write-TestText (Join-Path $directory 'backend\rve-backend.py') 'stable'
}
Write-TestText (Join-Path $base 'backend\rve-image-backend.py') 'old'
Write-TestText (Join-Path $target 'backend\rve-image-backend.py') 'new'

$previousPatches = @(0..5 | ForEach-Object {
    [ordered]@{
        baseVersion = "v$_"
        targetVersion = 'v' + ($_ + 1)
        path = "Backend/patches/v${_}_to_v$($_ + 1).7z"
        size = 100
        sha256 = ('a' * 64)
    }
})
$previousBaselines = @(0..5 | ForEach-Object {
    [ordered]@{
        version = "v$_"
        sentinels = @([ordered]@{
            path = 'backend/rve-backend.py'
            sha256 = (Get-FileHash -Algorithm SHA256 -LiteralPath (Join-Path $base 'backend\rve-backend.py')).Hash.ToLowerInvariant()
        })
    }
})
$previous = [ordered]@{
    schemaVersion = 1
    latestVersion = 'v6'
    full = [ordered]@{ path = 'Backend/old.7z'; size = 100; sha256 = ('a' * 64) }
    patches = $previousPatches
    legacyBaselines = $previousBaselines
}
$previousPath = Join-Path $root 'previous-channel.json'
Write-TestText $previousPath ($previous | ConvertTo-Json -Depth 8)
$archive = Join-Path $root 'full.7z'
& $ArchiveTool --create-7z (Join-Path $root 'target') $archive | Out-Host
if ($LASTEXITCODE -ne 0) { throw '测试完整包创建失败' }

& (Join-Path $PSScriptRoot 'prepare-backend-update.ps1') `
    -BaseRoot $base -TargetRoot $target -BaseVersion 'v6' -TargetVersion 'v7' `
    -FullArchive $archive -OutputRoot $output -ArchiveTool $ArchiveTool `
    -PreviousChannel $previousPath -SentinelPaths 'backend/rve-backend.py' | Out-Host

$channel = Get-Content -Raw -Encoding UTF8 (Join-Path $output 'channel.json') | ConvertFrom-Json
$actual = @($channel.patches | ForEach-Object { "$($_.baseVersion)->$($_.targetVersion)" })
$expected = @('v2->v3', 'v3->v4', 'v4->v5', 'v5->v6', 'v6->v7')
if (($actual -join ',') -ne ($expected -join ',')) {
    throw "最近五代补丁链错误：$($actual -join ',')"
}
$baselines = @($channel.legacyBaselines | ForEach-Object { $_.version })
if (($baselines -join ',') -ne ('v2,v3,v4,v5,v6')) {
    throw "旧版哨兵窗口错误：$($baselines -join ',')"
}
$core = Join-Path $root 'core'
New-Item -ItemType Directory -Force -Path $core | Out-Null
$exe = Join-Path $core 'videoenhancer.exe'
Copy-Item -LiteralPath $ArchiveTool -Destination $exe
Write-TestText (Join-Path $core 'python\python\python.exe') 'python'
Write-TestText (Join-Path $core 'python\backend\rve-backend.py') 'stable'
$markerPath = Join-Path $core 'python\.videoenhancer-backend.json'
Write-TestText $markerPath '{"version":"v2"}'
$status = (& $exe --backend-status --backend-channel (Join-Path $output 'channel.json') --json |
    Select-Object -Last 1) | ConvertFrom-Json
if ($LASTEXITCODE -ne 0 -or $status.mode -ne 'patch' -or $status.patchCount -ne 5) {
    throw '五代前的后端未选择完整补丁链'
}
Write-TestText $markerPath '{"version":"v1"}'
$status = (& $exe --backend-status --backend-channel (Join-Path $output 'channel.json') --json |
    Select-Object -Last 1) | ConvertFrom-Json
if ($LASTEXITCODE -ne 0 -or $status.mode -ne 'full' -or $status.patchCount -ne 0) {
    throw '超过五代窗口的后端未回退完整包'
}
Write-Output 'BACKEND_CHANNEL_HISTORY_TESTS_PASS|5'
