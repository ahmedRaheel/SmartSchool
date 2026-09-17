param(
    [string]$MigrationPrefix = "Initial",

    [ValidateSet("Development", "Staging", "Production")]
    [string]$Environment = "Development",

    [switch]$SkipRestore,
    [switch]$SkipBuild,
    [switch]$ContinueOnError,
    [switch]$Force
)

. "$PSScriptRoot/ModuleMigrationConfig.ps1"

function Test-ExistingModuleMigrations {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Module
    )

    $migrationDirectory = Join-Path `
        $SmartSchoolRepoRoot `
        "src/Modules/$Module/$SmartSchoolMigrationOutputDirectory"

    if (-not (Test-Path $migrationDirectory)) {
        return $false
    }

    $migrationFiles = Get-ChildItem `
        -Path $migrationDirectory `
        -Filter "*.cs" `
        -File `
        -ErrorAction SilentlyContinue

    return $migrationFiles.Count -gt 0
}

Write-Host ""
Write-Host "SmartSchool - First-Time Modular Migration Bootstrap" -ForegroundColor Cyan
Write-Host "Repository : $SmartSchoolRepoRoot"
Write-Host "Environment: $Environment"
Write-Host "Modules    : $($SmartSchoolModules.Count)"
Write-Host ""
Write-Warning "This bootstrap is intended for a NEW/EMPTY SmartSchool database."
Write-Warning "Do not use Initial* migrations against a database already created by SmartSchool.FreshInstall.sql unless it has been baselined first."

if (-not $Force) {
    $answer = Read-Host "Continue with first-time migration creation and database update? (Y/N)"
    if ($answer -notin @("Y", "y", "Yes", "yes")) {
        Write-Host "Cancelled."
        exit 0
    }
}

$env:ASPNETCORE_ENVIRONMENT = $Environment
$env:DOTNET_ENVIRONMENT = $Environment

Push-Location $SmartSchoolRepoRoot
try {
    Write-Host ""
    Write-Host "Checking dotnet-ef..." -ForegroundColor Cyan
    & dotnet ef --version
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet-ef is not available. Install/update it before running this script."
    }

    if (-not $SkipRestore) {
        Write-Host ""
        Write-Host "Restoring solution..." -ForegroundColor Cyan
        & dotnet restore
        if ($LASTEXITCODE -ne 0) {
            throw "dotnet restore failed."
        }
    }

    if (-not $SkipBuild) {
        Write-Host ""
        Write-Host "Building SmartSchool startup project once before scaffolding migrations..." -ForegroundColor Cyan
        & dotnet build $SmartSchoolStartupProject --no-restore
        if ($LASTEXITCODE -ne 0) {
            throw "SmartSchool build failed. Migrations were not started."
        }
    }

    $failed = @()
    $created = @()
    $skipped = @()
    $updated = @()

    foreach ($config in $SmartSchoolModules) {
        $module = $config.Name
        $migrationName = "$MigrationPrefix$module"

        Write-Host ""
        Write-Host "============================================================" -ForegroundColor DarkGray
        Write-Host "$module - $($config.Context)" -ForegroundColor Cyan
        Write-Host "============================================================" -ForegroundColor DarkGray

        try {
            if (Test-ExistingModuleMigrations -Module $module) {
                Write-Host "Existing migrations found for $module. Skipping '$migrationName' creation." -ForegroundColor Yellow
                $skipped += $module
            }
            else {
                & "$PSScriptRoot/Add-ModuleMigration.ps1" `
                    -Module $module `
                    -Name $migrationName `
                    -NoBuild

                if ($LASTEXITCODE -ne 0) {
                    throw "Migration creation failed for $module."
                }

                $created += $module
            }

            & "$PSScriptRoot/Update-ModuleDatabase.ps1" `
                -Module $module `
                -NoBuild

            if ($LASTEXITCODE -ne 0) {
                throw "Database update failed for $module."
            }

            $updated += $module
        }
        catch {
            $failed += $module
            Write-Error $_

            if (-not $ContinueOnError) {
                break
            }
        }
    }

    Write-Host ""
    Write-Host "==================== Migration Summary ====================" -ForegroundColor Cyan
    Write-Host "Created : $($created.Count) [$($created -join ', ')]"
    Write-Host "Skipped : $($skipped.Count) [$($skipped -join ', ')]"
    Write-Host "Updated : $($updated.Count) [$($updated -join ', ')]"

    if ($failed.Count -gt 0) {
        Write-Host "Failed  : $($failed.Count) [$($failed -join ', ')]" -ForegroundColor Red
        exit 1
    }

    Write-Host "Failed  : 0" -ForegroundColor Green
    Write-Host ""
    Write-Host "All SmartSchool business module migrations are initialized and applied." -ForegroundColor Green
}
finally {
    Pop-Location
}
