param(
    [switch]$IncludeIdentity,
    [switch]$Force,
    [switch]$WhatIf
)

$ErrorActionPreference = "Stop"

$repositoryRoot = Split-Path -Parent $PSScriptRoot
$modulesRoot = Join-Path $repositoryRoot "src\Modules"

if (-not (Test-Path $modulesRoot)) {
    throw "Modules folder was not found: $modulesRoot"
}

Write-Host ""
Write-Host "SmartSchool - Delete Existing EF Core Migrations" -ForegroundColor Cyan
Write-Host "Repository: $repositoryRoot" -ForegroundColor DarkGray
Write-Host ""

$migrationDirectories = Get-ChildItem `
    -Path $modulesRoot `
    -Directory `
    -Recurse |
    Where-Object {
        $_.FullName -match '[\\/]Persistence[\\/]Migrations[\\/]PostgreSql$'
    }

if (-not $IncludeIdentity) {
    $migrationDirectories = $migrationDirectories |
        Where-Object {
            $_.FullName -notmatch '[\\/]Modules[\\/]Identity[\\/]'
        }
}

if (-not $migrationDirectories) {
    Write-Host "No PostgreSQL migration directories were found." -ForegroundColor Yellow
    exit 0
}

Write-Host "Migration directories found:" -ForegroundColor Cyan

foreach ($directory in $migrationDirectories) {
    $relativePath = $directory.FullName.Substring($repositoryRoot.Length).TrimStart('\', '/')
    Write-Host "  $relativePath"
}

Write-Host ""

if (-not $Force -and -not $WhatIf) {
    $confirmation = Read-Host "Delete all migration files from these directories? Type YES to continue"

    if ($confirmation -cne "YES") {
        Write-Host "Cancelled. No migration files were deleted." -ForegroundColor Yellow
        exit 0
    }
}

$deletedFiles = 0
$processedDirectories = 0

foreach ($directory in $migrationDirectories) {
    $processedDirectories++

    $files = Get-ChildItem `
        -Path $directory.FullName `
        -File `
        -Recurse |
        Where-Object {
            $_.Name -ne ".gitkeep"
        }

    if (-not $files) {
        Write-Host "No migration files: $($directory.FullName)" -ForegroundColor DarkGray
        continue
    }

    foreach ($file in $files) {
        $relativeFile = $file.FullName.Substring($repositoryRoot.Length).TrimStart('\', '/')

        if ($WhatIf) {
            Write-Host "[WHATIF] Delete $relativeFile" -ForegroundColor Yellow
            continue
        }

        Remove-Item -LiteralPath $file.FullName -Force
        $deletedFiles++

        Write-Host "[DELETED] $relativeFile" -ForegroundColor Green
    }

    if (-not $WhatIf) {
        Get-ChildItem `
            -Path $directory.FullName `
            -Directory `
            -Recurse |
            Sort-Object FullName -Descending |
            ForEach-Object {
                if (-not (Get-ChildItem -LiteralPath $_.FullName -Force)) {
                    Remove-Item -LiteralPath $_.FullName -Force
                }
            }

        $gitKeep = Join-Path $directory.FullName ".gitkeep"

        if (-not (Test-Path $gitKeep)) {
            New-Item -ItemType File -Path $gitKeep -Force | Out-Null
        }
    }
}

Write-Host ""
Write-Host "============================================================" -ForegroundColor DarkGray
Write-Host "Migration cleanup summary" -ForegroundColor Cyan
Write-Host "============================================================" -ForegroundColor DarkGray
Write-Host "Directories processed: $processedDirectories"

if ($WhatIf) {
    Write-Host "WhatIf mode: no files were deleted." -ForegroundColor Yellow
} else {
    Write-Host "Migration files deleted: $deletedFiles" -ForegroundColor Green
}

if (-not $IncludeIdentity) {
    Write-Host "Identity migrations were preserved." -ForegroundColor Green
} else {
    Write-Host "Identity migrations were included in cleanup." -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Next step for a fresh database:" -ForegroundColor Cyan
Write-Host ".\scripts\Initialize-AllModuleMigrations.ps1 -Environment Development -Force"
