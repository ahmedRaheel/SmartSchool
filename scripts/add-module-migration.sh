#!/usr/bin/env bash
set -euo pipefail

script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
repo_root="$(cd "$script_dir/.." && pwd)"
cd "$repo_root"

if [[ $# -ne 2 ]]; then
  echo "Usage: $0 <Module> <MigrationName>"
  echo "Example: $0 Workflow InitialWorkflow"
  exit 1
fi

module="$1"
name="$2"

case "$module" in
  AICore) context="AICoreDbContext" ;;
  AIInquiry) context="AIInquiryDbContext" ;;
  AIParent) context="AIParentDbContext" ;;
  AIPrediction) context="AIPredictionDbContext" ;;
  AITutor) context="AITutorDbContext" ;;
  Activities) context="ActivitiesDbContext" ;;
  Admissions) context="AdmissionsDbContext" ;;
  Audit) context="AuditDbContext" ;;
  Communication) context="CommunicationDbContext" ;;
  Documents) context="DocumentsDbContext" ;;
  Examinations) context="ExaminationsDbContext" ;;
  Finance) context="FinanceDbContext" ;;
  HR) context="HRDbContext" ;;
  Inventory) context="InventoryDbContext" ;;
  Learning) context="LearningDbContext" ;;
  Library) context="LibraryDbContext" ;;
  Organization) context="OrganizationDbContext" ;;
  Payroll) context="PayrollDbContext" ;;
  Reference) context="ReferenceDbContext" ;;
  Students) context="StudentsDbContext" ;;
  Transport) context="TransportDbContext" ;;
  Workflow) context="WorkflowDbContext" ;;
  *)
    echo "Unsupported module: $module"
    echo "Supported modules: AICore AIInquiry AIParent AIPrediction AITutor Activities Admissions Audit Communication Documents Examinations Finance HR Inventory Learning Library Organization Payroll Reference Students Transport Workflow"
    exit 2
    ;;
esac

project="src/Modules/$module/SmartSchool.Modules.$module.csproj"
startup_project="src/SmartSchool.Api/SmartSchool.Api.csproj"

if [[ ! -f "$project" ]]; then
  echo "Module project not found: $repo_root/$project"
  exit 3
fi

if [[ ! -f "$startup_project" ]]; then
  echo "Startup project not found: $repo_root/$startup_project"
  exit 4
fi

echo "Creating migration '$name' for $module ($context)..."

dotnet ef migrations add "$name" \
  --context "$context" \
  --project "$project" \
  --startup-project "$startup_project" \
  --output-dir Persistence/Migrations/PostgreSql
