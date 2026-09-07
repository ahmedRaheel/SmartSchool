#!/usr/bin/env bash
set -euo pipefail
fail=0
check_zero() { label="$1"; shift; count=$(eval "$*" | wc -l); if [ "$count" -ne 0 ]; then echo "FAIL $label: $count"; fail=1; else echo "PASS $label"; fi; }
check_zero "Persistence contains Query/Command slices" "find src -path '*/Persistence/*' \( -name '*Query*.cs' -o -name '*Command*.cs' \) -print"
check_zero "AICore OllamaClient uses IConfiguration" "grep -l 'IConfiguration' src/Modules/AICore/Cag/OllamaClient.cs || true"
check_zero "AICore OllamaClient hides provider errors with EnsureSuccessStatusCode" "grep -l 'EnsureSuccessStatusCode' src/Modules/AICore/Cag/OllamaClient.cs || true"
grep -q '"/api/ai/model-config"' src/Modules/AICore/Features/ModelConfiguration/GetModelConfigurationPage.cs && echo 'PASS /api/ai/model-config compatibility endpoint' || { echo 'FAIL model-config endpoint'; fail=1; }
grep -q 'AiKnowledgeContribution' src/Modules/AICore/Features/KnowledgeDocument/UploadKnowledgePdf.cs && echo 'PASS AI knowledge upload policy' || { echo 'FAIL AI upload policy'; fail=1; }
exit $fail
