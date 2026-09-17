param(
    [Parameter(Mandatory = $true)]
    [ValidateSet(
        "AICore", "AIInquiry", "AIParent", "AIPrediction", "AITutor",
        "Activities", "Admissions", "Audit", "Communication", "Documents",
        "Examinations", "Finance", "HR", "Inventory", "Learning", "Library",
        "Organization", "Payroll", "Reference", "Students", "Transport", "Workflow")]
    [string]$Module,

    [Parameter(Mandatory = $true)]
    [string]$Name
)

$contexts = @{
    AICore = "AICoreDbContext"
    AIInquiry = "AIInquiryDbContext"
    AIParent = "AIParentDbContext"
    AIPrediction = "AIPredictionDbContext"
    AITutor = "AITutorDbContext"
    Activities = "ActivitiesDbContext"
    Admissions = "AdmissionsDbContext"
    Audit = "AuditDbContext"
    Communication = "CommunicationDbContext"
    Documents = "DocumentsDbContext"
    Examinations = "ExaminationsDbContext"
    Finance = "FinanceDbContext"
    HR = "HRDbContext"
    Inventory = "InventoryDbContext"
    Learning = "LearningDbContext"
    Library = "LibraryDbContext"
    Organization = "OrganizationDbContext"
    Payroll = "PayrollDbContext"
    Reference = "ReferenceDbContext"
    Students = "StudentsDbContext"
    Transport = "TransportDbContext"
    Workflow = "WorkflowDbContext"
}

$context = $contexts[$Module]
$project = "src/Modules/$Module/SmartSchool.Modules.$Module.csproj"
$startupProject = "src/SmartSchool.Api/SmartSchool.Api.csproj"

Write-Host "Creating migration '$Name' for $Module ($context)..."

dotnet ef migrations add $Name `
    --context $context `
    --project $project `
    --startup-project $startupProject `
    --output-dir Persistence/Migrations/PostgreSql

if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

Write-Host "Migration created successfully."
