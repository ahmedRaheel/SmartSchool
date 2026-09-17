param(
    [Parameter(Mandatory = $true)]
    [string]$Module,

    [switch]$NoBuild
)

. "$PSScriptRoot/ModuleMigrationConfig.ps1"

$config = Get-SmartSchoolModuleConfig -Module $Module
$arguments = @(
    "ef", "database", "update",
    "--context", $config.Context,
    "--project", $config.Project,
    "--startup-project", $SmartSchoolStartupProject
)

if ($NoBuild) {
    $arguments += "--no-build"
}

Push-Location $SmartSchoolRepoRoot
try {
    Write-Host "Applying migrations for $($config.Name) ($($config.Context))..." -ForegroundColor Cyan
    & dotnet @arguments

    if ($LASTEXITCODE -ne 0) {
        exit $LASTEXITCODE
    }

    Write-Host "$($config.Name) database migration completed successfully." -ForegroundColor Green
}
finally {
    Pop-Location
}
