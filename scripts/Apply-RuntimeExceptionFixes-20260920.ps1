param(
    [switch]$NoBuild
)

$ErrorActionPreference = "Stop"
$modules = @("Organization", "HR", "Documents", "Students", "Library", "AITutor", "Activities", "Finance")

foreach ($module in $modules) {
    Write-Host "Applying runtime-exception migrations for $module..." -ForegroundColor Cyan
    $arguments = @("-Module", $module)
    if ($NoBuild) { $arguments += "-NoBuild" }
    & "$PSScriptRoot/Update-ModuleDatabase.ps1" @arguments
    if ($LASTEXITCODE -ne 0) {
        throw "Migration update failed for module '$module' with exit code $LASTEXITCODE."
    }
}

Write-Host "Runtime-exception migrations completed." -ForegroundColor Green
