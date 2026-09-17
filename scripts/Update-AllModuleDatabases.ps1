param(
    [switch]$ContinueOnError,
    [switch]$NoBuild
)

. "$PSScriptRoot/ModuleMigrationConfig.ps1"

$failed = @()

foreach ($config in $SmartSchoolModules) {
    Write-Host ""
    Write-Host "============================================================" -ForegroundColor DarkGray
    Write-Host "Applying migrations for $($config.Name)" -ForegroundColor Cyan
    Write-Host "============================================================" -ForegroundColor DarkGray

    & "$PSScriptRoot/Update-ModuleDatabase.ps1" `
        -Module $config.Name `
        -NoBuild:$NoBuild

    if ($LASTEXITCODE -ne 0) {
        $failed += $config.Name

        if (-not $ContinueOnError) {
            Write-Error "Database update failed for $($config.Name)."
            exit $LASTEXITCODE
        }
    }
}

if ($failed.Count -gt 0) {
    Write-Error "Database update failed for: $($failed -join ', ')"
    exit 1
}

Write-Host ""
Write-Host "All business module databases were updated successfully." -ForegroundColor Green
