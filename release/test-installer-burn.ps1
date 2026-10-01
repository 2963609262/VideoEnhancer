param([string]$Installer = '')

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
if (-not $Installer) { $Installer = Join-Path $root 'Artifacts/VideoEnhancerInstaller.exe' }
$Installer = [IO.Path]::GetFullPath($Installer)
$probeRoot = Join-Path $root ('Artifacts/.refactor-tmp/burn-green-' + [guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $probeRoot | Out-Null
$hostRoot = Join-Path $probeRoot 'host with spaces'
$tempRoot = Join-Path $probeRoot 'temp'
New-Item -ItemType Directory -Path $hostRoot,$tempRoot | Out-Null
[IO.File]::WriteAllText((Join-Path $hostRoot 'FFmpegFreeUI.exe'), 'test host', [Text.UTF8Encoding]::new($false))

# 从最终构建清单取得本次安装器身份，不凭名称扫描或删除其他安装器状态。
$pdb = Join-Path $root 'installer/Bundle/obj/x64/Release/VideoEnhancerInstaller.wixpdb'
$archive = [IO.Compression.ZipFile]::OpenRead($pdb)
try {
    $reader = [IO.StreamReader]::new($archive.GetEntry('wix-burndata.xml').Open(), [Text.Encoding]::UTF8)
    try { [xml]$manifest = $reader.ReadToEnd() } finally { $reader.Dispose() }
} finally { $archive.Dispose() }
$registration = $manifest.BurnManifest.Registration
$package = $manifest.BurnManifest.Chain.ExePackage
if ($package.Cache -ne 'remove' -or $package.Permanent -ne 'yes') { throw '安装链未配置执行后清理' }
if ($manifest.BurnManifest.Log.Prefix) { throw '默认 Burn 日志未关闭' }
$cacheRoot = Join-Path ([Environment]::GetFolderPath('LocalApplicationData')) 'Package Cache'
$bundleCache = Join-Path $cacheRoot $registration.Code
$payloadCache = Join-Path $cacheRoot $package.CacheId
$uninstallKey = 'HKCU:/Software/Microsoft/Windows/CurrentVersion/Uninstall/' + $registration.Code
$dependencyKey = 'HKCU:/Software/Classes/Installer/Dependencies/' + $registration.ProviderKey

function Assert-NoBurnResidue {
    foreach ($path in @($bundleCache,$payloadCache,$uninstallKey,$dependencyKey)) {
        if (Test-Path -LiteralPath $path) { throw "Burn 留下本次安装状态：$path" }
    }
}

function Invoke-Burn([string]$folder, [string]$log = '') {
    $start = [Diagnostics.ProcessStartInfo]::new($Installer)
    $start.UseShellExecute = $false
    $start.CreateNoWindow = $true
    foreach ($arg in @('/quiet','/norestart',('InstallFolder=' + $folder))) { $start.ArgumentList.Add($arg) }
    if ($log) {
        $start.ArgumentList.Add('/log')
        $start.ArgumentList.Add($log)
    }
    $start.Environment['TEMP'] = $tempRoot
    $start.Environment['TMP'] = $tempRoot
    # 旧配置迁移仅接触测试目录；Burn 的缓存仍使用 Windows 当前用户的实际路径。
    $start.Environment['LOCALAPPDATA'] = Join-Path $probeRoot 'localappdata'
    $process = [Diagnostics.Process]::Start($start)
    $process.WaitForExit()
    $result = $process.ExitCode
    $process.Dispose()
    Assert-NoBurnResidue
    return $result
}

Assert-NoBurnResidue
if ((Invoke-Burn $hostRoot (Join-Path $probeRoot 'success.log')) -ne 0) { throw '外层 WiX 安装失败' }
$dll = Join-Path $hostRoot 'Plugin/videoenhancer.3fui.dll'
$exe = Join-Path $hostRoot 'Plugin/videoenhancer/videoenhancer.exe'
if ((Get-FileHash -LiteralPath $exe).Hash -ne (Get-FileHash -LiteralPath (Join-Path $root 'Artifacts/videoenhancer.exe')).Hash) { throw '外层安装后的运行文件哈希不一致' }
$dllHash = (Get-FileHash -LiteralPath $dll).Hash
$exeHash = (Get-FileHash -LiteralPath $exe).Hash
if ((Invoke-Burn (Join-Path $probeRoot 'invalid') (Join-Path $probeRoot 'invalid.log')) -eq 0) { throw '外层 WiX 未拒绝无效目录' }
if (Test-Path -LiteralPath (Join-Path $probeRoot 'invalid')) { throw '无效目录被写入' }
$env:VIDEOENHANCER_INSTALL_FAIL_AFTER = '2'
try {
    if ((Invoke-Burn $hostRoot (Join-Path $probeRoot 'rollback.log')) -eq 0) { throw '故障注入后没有失败' }
} finally { Remove-Item Env:VIDEOENHANCER_INSTALL_FAIL_AFTER }
if ((Get-FileHash -LiteralPath $dll).Hash -ne $dllHash -or (Get-FileHash -LiteralPath $exe).Hash -ne $exeHash) { throw '外层失败后没有恢复原文件' }
$errorLog = Join-Path $hostRoot 'Plugin/videoenhancer/logs/installer.log'
if (-not (Test-Path -LiteralPath $errorLog) -or -not (Get-Content -Raw -Encoding UTF8 $errorLog).Contains('System.IO.')) { throw '插件目录缺少完整安装异常' }
if ((Invoke-Burn $hostRoot) -ne 0) { throw '关闭默认日志后无法正常安装' }
if (@(Get-ChildItem -LiteralPath $tempRoot -Recurse -File -Filter '*.log').Count -ne 0) { throw '临时目录仍有默认安装日志' }
Write-Host ('BURN_GREEN_TESTS_PASS|success|invalid-root|rollback|plugin-error-log|no-default-log|no-package-cache|no-bundle-cache|no-registration|' + $probeRoot)
