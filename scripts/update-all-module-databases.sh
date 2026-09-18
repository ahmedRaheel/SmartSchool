#!/usr/bin/env bash
set -euo pipefail

script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

modules=(
  AICore AIInquiry AIParent AIPrediction AITutor
  Activities Admissions Audit Communication Documents
  Examinations Finance HR Inventory Learning Library
  Organization Payroll Reference Students Transport Workflow
)

for module in "${modules[@]}"; do
  echo
  echo "============================================================"
  echo "Applying migrations for ${module}"
  echo "============================================================"
  "$script_dir/update-module-database.sh" "$module"
done

echo
echo "All business module databases were updated successfully."
