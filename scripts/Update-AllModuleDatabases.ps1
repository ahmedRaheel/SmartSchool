param(
    [switch]$NoBuild,
    [switch]$FailAtEnd,
    [switch]$StopOnError
)

. "$PSScriptRoot/ModuleMigrationConfig.ps1"

$successful = [System.Collections.Generic.List[string]]::new()
$failed = [System.Collections.Generic.List[string]]::new()

foreach ($config in $SmartSchoolModules) {
    Write-Host ""
    Write-Host "============================================================" -ForegroundColor DarkGray
    Write-Host "Applying migrations for $($config.Name)" -ForegroundColor Cyan
    Write-Host "============================================================" -ForegroundColor DarkGray

    & "$PSScriptRoot/Update-ModuleDatabase.ps1" `
        -Module $config.Name `
        -NoBuild:$NoBuild

    $exitCode = $LASTEXITCODE
    if ($null -eq $exitCode) {
        $exitCode = 0
    }

    if ($exitCode -eq 0) {
        $successful.Add($config.Name)
        Write-Host "[OK] $($config.Name)" -ForegroundColor Green
        continue
    }

    $failed.Add($config.Name)
    Write-Warning ("Migration update failed for module '{0}' with exit code {1}. Skipping and continuing." -f $config.Name, $exitCode)

    if ($StopOnError) {
        Write-Error "Stopping because -StopOnError was specified."
        exit $exitCode
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

    Write-Warning ("{0} module(s) could not be updated, but every remaining module was processed." -f $failed.Count)

    if ($FailAtEnd) {
        exit 1
    }

    exit 0
}

Write-Host ""
Write-Host "All business module databases were updated successfully." -ForegroundColor Green
exit 0
