#!/usr/bin/env bash
set -euo pipefail
ROOT="${1:-src}"
violations=$(grep -RInE 'interface I[A-Za-z0-9_]*(Query|Command)\b' "$ROOT" --include='*.cs' | grep -v '/Modules/Identity/' | grep -v '/SmartSchool.Identity.Api/' || true)
if [[ -n "$violations" ]]; then
  echo "Architecture violation: shared I*Query/I*Command interfaces found:" >&2
  echo "$violations" >&2
  exit 1
fi
echo "PASS: no non-Identity shared I*Query/I*Command interfaces found."
