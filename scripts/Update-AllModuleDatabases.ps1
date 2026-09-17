param(
    [switch]$NoBuild,
    [switch]$FailAtEnd,
    [switch]$StopOnError
)

. "$PSScriptRoot/ModuleMigrationConfig.ps1"

$successful = @()
$failed = @()

foreach ($config in $SmartSchoolModules) {
    Write-Host ""
    Write-Host "============================================================" -ForegroundColor DarkGray
    Write-Host "Applying migrations for $($config.Name)" -ForegroundColor Cyan
    Write-Host "============================================================" -ForegroundColor DarkGray

    try {
        & "$PSScriptRoot/Update-ModuleDatabase.ps1" `
            -Module $config.Name `
            -NoBuild:$NoBuild

        $exitCode = $LASTEXITCODE

        if ($null -eq $exitCode) {
            $exitCode = 0
        }

        if ($exitCode -ne 0) {
            $failed += $config.Name

            Write-Warning (
                "Migration update failed for module '{0}'. " +
                "Skipping this module and continuing..."
            ) -f $config.Name

            if ($StopOnError) {
                Write-Error "Stopping because -StopOnError was specified."
                exit $exitCode
            }

            continue
        }

        $successful += $config.Name

        Write-Host (
            "Migration update completed for {0}."
        ) -f $config.Name -ForegroundColor Green
    }
    catch {
        $failed += $config.Name

        Write-Warning (
            "Unexpected error while updating module '{0}': {1}"
        ) -f $config.Name, $_.Exception.Message

        Write-Warning (
            "Skipping module '{0}' and continuing..."
        ) -f $config.Name

        if ($StopOnError) {
            throw
        }

        continue
    }
}

Write-Host ""
Write-Host "============================================================" -ForegroundColor DarkGray
Write-Host "SmartSchool Migration Summary" -ForegroundColor Cyan
Write-Host "============================================================" -ForegroundColor DarkGray

Write-Host ""
Write-Host "Successful modules: $($successful.Count)" -ForegroundColor Green

foreach ($module in $successful) {
    Write-Host "  [OK] $module" -ForegroundColor Green
}

if ($failed.Count -gt 0) {
    Write-Host ""
    Write-Host "Failed / skipped modules: $($failed.Count)" -ForegroundColor Yellow

    foreach ($module in $failed) {
        Write-Host "  [SKIPPED] $module" -ForegroundColor Yellow
    }
}

Write-Host ""

if ($failed.Count -eq 0) {
    Write-Host (
        "All business module databases were updated successfully."
    ) -ForegroundColor Green

    exit 0
}

Write-Warning (
    "{0} module(s) could not be updated, but all remaining modules were processed."
) -f $failed.Count

if ($FailAtEnd) {
    exit 1
}

exit 0