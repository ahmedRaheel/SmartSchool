param(
    [string]$BaseUrl = "http://localhost:61342",
    [ValidateSet("All", "ReadOnly")]
    [string]$Mode = "All",
    [string]$Settings = "",
    [string]$Token = $env:SMARTSCHOOL_API_TOKEN,
    [string]$ReportDirectory = "artifacts/api-smoke",
    [int]$TimeoutSeconds = 30,
    [switch]$NoDelete,
    [switch]$Strict,
    [switch]$IgnoreTlsErrors,
    [switch]$NoBuild
)

$ErrorActionPreference = "Stop"

$repositoryRoot = Split-Path -Parent $PSScriptRoot
$testerProject = Join-Path `
    $repositoryRoot `
    "tools\SmartSchool.ApiSmokeTester\SmartSchool.ApiSmokeTester.csproj"

if (-not (Test-Path $testerProject)) {
    throw "API smoke tester project was not found: $testerProject"
}

if (-not [string]::IsNullOrWhiteSpace($Token)) {
    $env:SMARTSCHOOL_API_TOKEN = $Token
}

$arguments = @(
    "run",
    "--project",
    $testerProject
)

if ($NoBuild) {
    $arguments += "--no-build"
}

$arguments += "--"

if (-not [string]::IsNullOrWhiteSpace($Settings)) {
    $settingsPath = $Settings

    if (-not [System.IO.Path]::IsPathRooted($settingsPath)) {
        $settingsPath = Join-Path $repositoryRoot $settingsPath
    }

    $arguments += @("--settings", $settingsPath)
}

$arguments += @(
    "--base-url",
    $BaseUrl,
    "--report-dir",
    (Join-Path $repositoryRoot $ReportDirectory),
    "--timeout",
    $TimeoutSeconds.ToString()
)

if ($Mode -eq "ReadOnly") {
    $arguments += "--read-only"
}
else {
    $arguments += "--all"
}

if ($NoDelete) {
    $arguments += "--no-delete"
}

if ($Strict) {
    $arguments += "--strict"
}

if ($IgnoreTlsErrors) {
    $arguments += "--ignore-tls-errors"
}

Write-Host "" 
Write-Host "SmartSchool API endpoint automation" -ForegroundColor Cyan
Write-Host "Base URL : $BaseUrl"
Write-Host "Mode     : $Mode"
Write-Host "Token    : $(if ([string]::IsNullOrWhiteSpace($env:SMARTSCHOOL_API_TOKEN)) { 'NOT SET' } else { 'configured' })"
Write-Host ""

& dotnet @arguments

exit $LASTEXITCODE
