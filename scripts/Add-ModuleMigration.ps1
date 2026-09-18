param(
    [Parameter(Mandatory = $true)]
    [string]$Module,

    [Parameter(Mandatory = $true)]
    [string]$Name,

    [switch]$NoBuild
)

. "$PSScriptRoot/ModuleMigrationConfig.ps1"

$config = Get-SmartSchoolModuleConfig -Module $Module
$arguments = @(
    "ef", "migrations", "add", $Name,
    "--context", $config.Context,
    "--project", $config.Project,
    "--startup-project", $SmartSchoolStartupProject,
    "--output-dir", $SmartSchoolMigrationOutputDirectory
)

if ($NoBuild) {
    $arguments += "--no-build"
}

Push-Location $SmartSchoolRepoRoot
try {
    Write-Host "Creating migration '$Name' for $($config.Name) ($($config.Context))..." -ForegroundColor Cyan
    & dotnet @arguments

    if ($LASTEXITCODE -ne 0) {
        exit $LASTEXITCODE
    }

    Write-Host "Migration created successfully for $($config.Name)." -ForegroundColor Green
}
finally {
    Pop-Location
}
