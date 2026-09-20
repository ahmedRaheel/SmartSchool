param(
    [string]$BaseUrl = "http://localhost:7001",
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

$expectedTesterVersion = "2026.09.19-env-token.1"
$testerVersionFile = Join-Path `
    $repositoryRoot `
    "tools\SmartSchool.ApiSmokeTester\SMOKE_TESTER_VERSION.txt"

if (-not (Test-Path $testerVersionFile)) {
    throw "Smoke tester version file is missing. Replace the complete tools\SmartSchool.ApiSmokeTester folder from the environment-token package."
}

$actualTesterVersion = (Get-Content $testerVersionFile -Raw).Trim()
if ($actualTesterVersion -ne $expectedTesterVersion) {
    throw "Smoke tester version mismatch. Script expects $expectedTesterVersion but tool is $actualTesterVersion."
}

if (-not (Test-Path $testerProject)) {
    throw "API smoke tester project was not found: $testerProject"
}

if ([string]::IsNullOrWhiteSpace($Token)) {
    throw "SMARTSCHOOL_API_TOKEN is required. Set `$env:SMARTSCHOOL_API_TOKEN to a valid access token before running the test."
}

$env:SMARTSCHOOL_API_TOKEN = $Token.Trim()

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
Write-Host "Base URL       : $BaseUrl"
Write-Host "Mode           : $Mode"
Write-Host "Authentication : SMARTSCHOOL_API_TOKEN"
Write-Host "Fixture        : disabled"
Write-Host ""

& dotnet @arguments

exit $LASTEXITCODE
