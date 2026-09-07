#!/usr/bin/env bash
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"
fail=0
while IFS= read -r f; do
  case "$f" in *"/Identity/"*) continue;; esac
  if [[ "$f" == *"FeaturePersistence.cs" || "$f" == *"/DataAccess/"* ]]; then echo "FAIL grouped persistence: $f"; fail=1; fi
done < <(find src/Modules -type f -name '*.cs')
# Any feature-owned Query/Command interface must be in a file whose path contains its use-case stem.
while IFS=: read -r f decl; do
  case "$f" in *"/Identity/"*|*"/Persistence/"*) continue;; esac
  name=$(printf '%s' "$decl" | sed -E 's/.*interface (I[A-Za-z0-9_]+).*/\1/')
  stem=${name#I}; stem=${stem%Query}; stem=${stem%Command}
  [[ "$f" == *"/$stem/"* || "$f" == *"/$stem.cs" ]] || { echo "FAIL interface not physically owned by feature: $name in $f"; fail=1; }
done < <(grep -RHE '^public interface I[A-Za-z0-9_]+(Query|Command)' src/Modules --include='*.cs' || true)
[[ $fail -eq 0 ]] && echo 'PASS: physical feature ownership outside Identity'
exit $fail
