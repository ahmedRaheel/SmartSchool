param(
    [string]$ContainerName = "postgres",
    [string]$Database = "smartschool_pro",
    [string]$DatabaseUser = "postgres"
)

$ErrorActionPreference = "Stop"

$repositoryRoot = Split-Path -Parent $PSScriptRoot
$sqlFile = Join-Path $repositoryRoot "database\bootstrap\00_CreatePlatformInfrastructure.PostgreSql.sql"

if (-not (Test-Path $sqlFile)) {
    throw "Platform bootstrap SQL was not found: $sqlFile"
}

Write-Host "Creating SmartSchool platform infrastructure..." -ForegroundColor Cyan
Write-Host "Container: $ContainerName" -ForegroundColor DarkGray
Write-Host "Database:  $Database" -ForegroundColor DarkGray

$sql = Get-Content $sqlFile -Raw
$sql | docker exec -i $ContainerName psql -v ON_ERROR_STOP=1 -U $DatabaseUser -d $Database

if ($LASTEXITCODE -ne 0) {
    throw "Platform infrastructure initialization failed with exit code $LASTEXITCODE."
}

Write-Host "Platform infrastructure initialized successfully." -ForegroundColor Green
Write-Host "Created/verified: platform.business_number_sequence" -ForegroundColor Green
