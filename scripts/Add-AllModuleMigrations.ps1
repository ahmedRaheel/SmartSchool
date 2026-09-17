param(
    [string]$NamePrefix = "Initial",
    [switch]$ContinueOnError,
    [switch]$NoBuild
)

. "$PSScriptRoot/ModuleMigrationConfig.ps1"

$failed = @()

foreach ($config in $SmartSchoolModules) {
    $migrationName = "$NamePrefix$($config.Name)"

    Write-Host ""
    Write-Host "============================================================" -ForegroundColor DarkGray
    Write-Host "Creating $migrationName for $($config.Name)" -ForegroundColor Cyan
    Write-Host "============================================================" -ForegroundColor DarkGray

    & "$PSScriptRoot/Add-ModuleMigration.ps1" `
        -Module $config.Name `
        -Name $migrationName `
        -NoBuild:$NoBuild

    if ($LASTEXITCODE -ne 0) {
        $failed += $config.Name

        if (-not $ContinueOnError) {
            Write-Error "Migration generation failed for $($config.Name)."
            exit $LASTEXITCODE
        }
    }
}

if ($failed.Count -gt 0) {
    Write-Error "Migration generation failed for: $($failed -join ', ')"
    exit 1
}

Write-Host ""
Write-Host "All business module migrations were generated successfully." -ForegroundColor Green
