$ErrorActionPreference = "Stop"

Write-Host "Removing bin and obj directories..."
Get-ChildItem -Path $PSScriptRoot\.. -Recurse -Directory |
    Where-Object { $_.Name -in @("bin", "obj") } |
    Remove-Item -Recurse -Force

Write-Host "Clearing NuGet locals..."
dotnet nuget locals all --clear

Write-Host "Restoring..."
dotnet restore "$PSScriptRoot\..\SmartSchool.slnx"

Write-Host "Building..."
dotnet build "$PSScriptRoot\..\SmartSchool.slnx" --no-restore
