#!/usr/bin/env bash
set -euo pipefail
ROOT="${1:-src}"
fail=0
zero() {
  local label="$1"; shift
  local output
  output=$(eval "$*" || true)
  if [[ -n "$output" ]]; then
    echo "FAIL: $label" >&2
    echo "$output" >&2
    fail=1
  else
    echo "PASS: $label"
  fi
}
zero "non-Identity shared I*Query/I*Command interfaces" "grep -RInE --include='*.cs' 'interface I[A-Za-z0-9_]*(Query|Command)\\b' '$ROOT' | grep -v '/Modules/Identity/' | grep -v '/SmartSchool.Identity.Api/'"
zero "non-Identity module Persistence contains anything except EF configurations" "find '$ROOT/Modules' -type f -path '*/Persistence/*' -name '*.cs' ! -path '*/Persistence/Configurations/*' ! -path '$ROOT/Modules/Identity/*' -print"
zero "sync-over-async (.Result/.Wait)" "grep -RInE --include='*.cs' '\\.Result([;,) ]|$)|\\.Wait\\(' '$ROOT/Modules' '$ROOT/BuildingBlocks' '$ROOT/SmartSchool.Api' | grep -v '/Modules/Identity/' | grep -v '/SmartSchool.Identity.Api/'"
zero "async void" "grep -RIn --include='*.cs' 'async void' '$ROOT/Modules' '$ROOT/BuildingBlocks' '$ROOT/SmartSchool.Api' | grep -v '/Modules/Identity/' | grep -v '/SmartSchool.Identity.Api/'"
zero "direct new HttpClient" "grep -RIn --include='*.cs' 'new HttpClient(' '$ROOT/Modules' '$ROOT/BuildingBlocks' '$ROOT/SmartSchool.Api' | grep -v '/Modules/Identity/' | grep -v '/SmartSchool.Identity.Api/'"
zero "hidden HTTP failures via EnsureSuccessStatusCode" "grep -RIn --include='*.cs' 'EnsureSuccessStatusCode' '$ROOT/Modules' '$ROOT/BuildingBlocks' '$ROOT/SmartSchool.Api' | grep -v '/Modules/Identity/' | grep -v '/SmartSchool.Identity.Api/'"
zero "direct DateTime.Now/UtcNow (use TimeProvider)" "grep -RInE --include='*.cs' 'DateTime\\.(Now|UtcNow)' '$ROOT/Modules' '$ROOT/BuildingBlocks' '$ROOT/SmartSchool.Api' | grep -v '/Modules/Identity/' | grep -v '/SmartSchool.Identity.Api/'"
zero "throw ex loses original stack" "grep -RIn --include='*.cs' 'throw ex;' '$ROOT/Modules' '$ROOT/BuildingBlocks' '$ROOT/SmartSchool.Api' | grep -v '/Modules/Identity/' | grep -v '/SmartSchool.Identity.Api/'"
exit "$fail"
