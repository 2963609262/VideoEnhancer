# 一键发布：把 outputs 中的产物发布到版本存档目录 + 各运行目录
# 规则：每个版本更新都发布到 C:\Users\ARXChem\Documents\LakeUI-2\videoenhancer.3fui\<版本>\
param(
    # 留空时自动读取 VideoEnhancerPlugin.vbproj 的 Version。
    [string]$Version = ''
)
$ErrorActionPreference = 'Stop'
$base = Split-Path -Parent $MyInvocation.MyCommand.Path

function Get-ProjectVersion([string]$projectPath) {
    $document = [System.Xml.XmlDocument]::new()
    $document.Load($projectPath)
    $nodes = @($document.SelectNodes('/Project/PropertyGroup/Version'))
    if ($nodes.Count -ne 1 -or [string]::IsNullOrWhiteSpace($nodes[0].InnerText)) {
        throw "项目必须声明且只能声明一个 Version：$projectPath"
    }
    return $nodes[0].InnerText.Trim()
}

$pluginProject = Join-Path $base 'VideoEnhancerPlugin\VideoEnhancerPlugin.vbproj'
$cliProject = Join-Path $base 'cli\VideoEnhancer.csproj'
$pluginVersion = Get-ProjectVersion $pluginProject
$cliVersion = Get-ProjectVersion $cliProject
if (-not $Version) { $Version = $pluginVersion }
if ($pluginVersion -ne $Version -or $cliVersion -ne $Version) {
    throw "项目版本不一致：插件=$pluginVersion，CLI=$cliVersion，部署版本=$Version"
}
$archiveRoot = 'C:\Users\ARXChem\Documents\LakeUI-2\videoenhancer.3fui'
$archive = Join-Path $archiveRoot $Version
$artifactsRoot = Join-Path $base 'Artifacts'
$installerArtifact = Join-Path $artifactsRoot 'VideoEnhancerInstaller.exe'
$manualArtifact = Join-Path $artifactsRoot 'VideoEnhancer.zip'
$pluginDll = Join-Path $base 'VideoEnhancerPlugin\obj\plugin-artifact\videoenhancer.3fui.dll'
New-Item -ItemType Directory -Force -Path $archive | Out-Null
New-Item -ItemType Directory -Force -Path (Join-Path $archive 'cli') | Out-Null
New-Item -ItemType Directory -Force -Path (Join-Path $archive 'VideoEnhancerPlugin') | Out-Null

# 1) 主程序产物（安装程序 + 手动安装包）
Copy-Item -LiteralPath $installerArtifact -Destination (Join-Path $archive 'VideoEnhancerInstaller.exe') -Force
Copy-Item -LiteralPath $manualArtifact -Destination (Join-Path $archive 'VideoEnhancer.zip') -Force
Copy-Item -LiteralPath (Join-Path $base 'VideoEnhancer.slnx') -Destination (Join-Path $archive 'VideoEnhancer.slnx') -Force

# 2) CLI 源码（Program.cs / README / csproj）
Copy-Item -LiteralPath (Join-Path $base 'cli\Program.cs') -Destination (Join-Path $archive 'cli\Program.cs') -Force
Copy-Item -LiteralPath (Join-Path $base 'cli\README.md') -Destination (Join-Path $archive 'cli\README.md') -Force
Copy-Item -LiteralPath (Join-Path $base 'cli\VideoEnhancer.csproj') -Destination (Join-Path $archive 'cli\VideoEnhancer.csproj') -Force

# 3) 插件源码、项目文件与说明
$pluginSrc = Join-Path $base 'VideoEnhancerPlugin'
$pluginDst = Join-Path $archive 'VideoEnhancerPlugin'
Get-ChildItem -LiteralPath $pluginSrc -File | ForEach-Object {
    Copy-Item -LiteralPath $_.FullName -Destination (Join-Path $pluginDst $_.Name) -Force
}
$assetSrc = Join-Path $pluginSrc 'assets'
if (Test-Path -LiteralPath $assetSrc) {
    Copy-Item -LiteralPath $assetSrc -Destination $pluginDst -Recurse -Force
}

# 4) 部署脚本本身
Copy-Item -LiteralPath $MyInvocation.MyCommand.Path -Destination (Join-Path $archive 'deploy.ps1') -Force

# 5) 插件 DLL 复制到运行目录：开发版 GUI 插件目录 + 最新发布版 ReadyToRun 插件目录
$pluginTargets = @(
    'C:\Users\ARXChem\Documents\LakeUIApps\Video Enhancer GUI\Plugin\videoenhancer.3fui.dll',
    'C:\PortableSoft\FFmpegFreeUI ReadyToRun x64\plugin\videoenhancer.3fui.dll'
)
foreach ($t in $pluginTargets) {
    try {
        New-Item -ItemType Directory -Force -Path (Split-Path -Parent $t) | Out-Null
        Copy-Item -LiteralPath $pluginDll -Destination $t -Force
        Write-Host "  已复制插件到 $t"
    } catch {
        Write-Host "  插件复制失败（跳过）：$t"
        Write-Host "    $($_.Exception.Message)"
    }
}

Write-Host ''
Write-Host "已发布 v$Version 到：$archive"
Write-Host '  插件已复制到 Video Enhancer GUI\Plugin 与 FFmpegFreeUI ReadyToRun x64\plugin。'
