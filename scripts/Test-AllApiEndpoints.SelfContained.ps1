param(
    [string]$BaseUrl = "http://localhost:7001",
    [string]$IdentityUrl = "http://localhost:7101",
    [ValidateSet("All", "ReadOnly")]
    [string]$Mode = "All",
    [string]$Settings = "",
    [string]$ReportDirectory = "artifacts/api-smoke",
    [int]$TimeoutSeconds = 30,
    [switch]$NoDelete,
    [switch]$Strict,
    [switch]$IgnoreTlsErrors,
    [switch]$NoBuild,
    [switch]$NoFixture,
    [switch]$KeepFixture
)

$ErrorActionPreference = "Stop"

$repositoryRoot = Split-Path -Parent $PSScriptRoot
$testerProject = Join-Path `
    $repositoryRoot `
    "tools\SmartSchool.ApiSmokeTester\SmartSchool.ApiSmokeTester.csproj"

if (-not (Test-Path $testerProject)) {
    throw "API smoke tester project was not found: $testerProject"
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
    "--identity-url",
    $IdentityUrl,
    "--repo-root",
    $repositoryRoot,
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

if ($NoFixture) {
    $arguments += "--no-fixture"
}

if ($KeepFixture) {
    $arguments += "--keep-fixture"
}

Write-Host ""
Write-Host "SmartSchool API endpoint automation" -ForegroundColor Cyan
Write-Host "Base URL     : $BaseUrl"
Write-Host "Identity URL : $IdentityUrl"
Write-Host "Mode         : $Mode"
Write-Host "Fixture      : $(if ($NoFixture) { 'disabled' } else { 'self-contained + auto-cleanup' })"
Write-Host ""

& dotnet @arguments

exit $LASTEXITCODE
