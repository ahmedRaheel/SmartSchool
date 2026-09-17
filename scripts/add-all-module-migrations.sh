#!/usr/bin/env bash
set -euo pipefail

script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
prefix="${1:-Initial}"

modules=(
  AICore AIInquiry AIParent AIPrediction AITutor
  Activities Admissions Audit Communication Documents
  Examinations Finance HR Inventory Learning Library
  Organization Payroll Reference Students Transport Workflow
)

for module in "${modules[@]}"; do
  echo
  echo "============================================================"
  echo "Creating ${prefix}${module} for ${module}"
  echo "============================================================"
  "$script_dir/add-module-migration.sh" "$module" "${prefix}${module}"
done

echo
echo "All business module migrations were generated successfully."
