# Shared module migration configuration for SmartSchool.
# Keep this file as the single source of truth for module migration tooling.

$SmartSchoolRepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
$SmartSchoolStartupProject = "src/SmartSchool.Api/SmartSchool.Api.csproj"
$SmartSchoolMigrationOutputDirectory = "Persistence/Migrations/PostgreSql"

# Foundation modules are intentionally listed before modules that commonly depend on them.
$SmartSchoolModules = @(
    [pscustomobject]@{ Name = "Reference";     Context = "ReferenceDbContext" },
    [pscustomobject]@{ Name = "Organization";  Context = "OrganizationDbContext" },
    [pscustomobject]@{ Name = "Documents";     Context = "DocumentsDbContext" },
    [pscustomobject]@{ Name = "Admissions";    Context = "AdmissionsDbContext" },
    [pscustomobject]@{ Name = "Students";      Context = "StudentsDbContext" },
    [pscustomobject]@{ Name = "Learning";      Context = "LearningDbContext" },
    [pscustomobject]@{ Name = "Examinations";  Context = "ExaminationsDbContext" },
    [pscustomobject]@{ Name = "Activities";    Context = "ActivitiesDbContext" },
    [pscustomobject]@{ Name = "Finance";       Context = "FinanceDbContext" },
    [pscustomobject]@{ Name = "HR";            Context = "HRDbContext" },
    [pscustomobject]@{ Name = "Payroll";       Context = "PayrollDbContext" },
    [pscustomobject]@{ Name = "Inventory";     Context = "InventoryDbContext" },
    [pscustomobject]@{ Name = "Library";       Context = "LibraryDbContext" },
    [pscustomobject]@{ Name = "Transport";     Context = "TransportDbContext" },
    [pscustomobject]@{ Name = "Communication"; Context = "CommunicationDbContext" },
    [pscustomobject]@{ Name = "Workflow";      Context = "WorkflowDbContext" },
    [pscustomobject]@{ Name = "Audit";         Context = "AuditDbContext" },
    [pscustomobject]@{ Name = "AICore";        Context = "AICoreDbContext" },
    [pscustomobject]@{ Name = "AIInquiry";     Context = "AIInquiryDbContext" },
    [pscustomobject]@{ Name = "AIParent";      Context = "AIParentDbContext" },
    [pscustomobject]@{ Name = "AIPrediction";  Context = "AIPredictionDbContext" },
    [pscustomobject]@{ Name = "AITutor";       Context = "AITutorDbContext" }
)

function Get-SmartSchoolModuleConfig {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Module
    )

    $config = $SmartSchoolModules | Where-Object { $_.Name -eq $Module } | Select-Object -First 1

    if ($null -eq $config) {
        $validModules = ($SmartSchoolModules.Name -join ", ")
        throw "Unknown SmartSchool module '$Module'. Valid modules: $validModules"
    }

    $config | Add-Member -NotePropertyName Project -NotePropertyValue "src/Modules/$($config.Name)/SmartSchool.Modules.$($config.Name).csproj" -Force
    return $config
}
