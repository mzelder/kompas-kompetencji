param(
    [Parameter(Mandatory)] [string] $AppName,
    [string] $ResourceGroup = "kompas-kompetencji",
    [string] $Configuration = "Release"
)

$ErrorActionPreference = "Stop"

$root = Split-Path $PSScriptRoot -Parent
$publishDir = Join-Path $root "publish"
$zipPath = Join-Path $root "publish.zip"

if (Test-Path $publishDir) { Remove-Item $publishDir -Recurse -Force }
if (Test-Path $zipPath) { Remove-Item $zipPath -Force }

dotnet publish (Join-Path $root "src/KompasKompetencji.Web") -c $Configuration -r linux-x64 --self-contained -o $publishDir
if ($LASTEXITCODE -ne 0) { throw "dotnet publish nie powiodl sie." }

Add-Type -AssemblyName System.IO.Compression.FileSystem
[System.IO.Compression.ZipFile]::CreateFromDirectory($publishDir, $zipPath)

az webapp deploy --name $AppName --resource-group $ResourceGroup --src-path $zipPath --type zip --clean true --restart true

Write-Host "Wdrozone: https://$AppName.azurewebsites.net"
