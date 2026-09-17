#!/usr/bin/env bash
set -euo pipefail

script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
repo_root="$(cd "$script_dir/.." && pwd)"
cd "$repo_root"

if [[ $# -ne 1 ]]; then
  echo "Usage: $0 <Module>"
  echo "Example: $0 Workflow"
  exit 1
fi

module="$1"

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
    exit 2
    ;;
esac

project="src/Modules/$module/SmartSchool.Modules.$module.csproj"
startup_project="src/SmartSchool.Api/SmartSchool.Api.csproj"

echo "Applying migrations for $module ($context)..."

dotnet ef database update \
  --context "$context" \
  --project "$project" \
  --startup-project "$startup_project"
