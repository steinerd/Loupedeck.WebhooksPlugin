$version = "1.0.1"
$project = "Webhooks"
$dllName = "WebhooksPlugin.dll"
$dllPath = "$($env:LOCALAPPDATA)\Loupedeck\Plugins\$project"
$buildPath = ".builds"
$outputFileName = "$project"
$zipPath = "$buildPath\$outputFileName.zip"
$pluginName = "$outputFileName.lplug4"
$loupedeckYaml = "LoupedeckPackage.yaml"
$cwd = Get-Location

New-Item -Path "$buildPath" -Force -Name "bin" -ItemType "directory" > $null
New-Item -Path "$buildPath" -Force -Name "metadata" -ItemType "directory" > $null

Copy-Item "$loupedeckYaml" -Force -Destination "$buildPath\metadata\$loupedeckYaml" > $null
Copy-Item "$dllPath\$dllName" -Force -Destination "$buildPath\bin\$dllName" > $null

$compress = @{
	Path = "$buildPath\*"
	CompressionLevel = "Fastest"
	DestinationPath = $zipPath
}
Compress-Archive @Compress > $null

Rename-Item $zipPath -Force -NewName $pluginName > $null