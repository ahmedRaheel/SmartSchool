#!/usr/bin/env bash
set -euo pipefail
root="$(cd "$(dirname "$0")/.." && pwd)"
cd "$root"
fail=0
check_absent(){ label="$1"; shift; out=$(eval "$*" || true); if [ -n "$out" ]; then echo "FAIL: $label"; echo "$out"; fail=1; else echo "PASS: $label"; fi; }
check_absent "DataAccess folders" "find src/Modules -type d -iname 'DataAccess' -print"
check_absent "Reader/Writer/ReadData/WriteData abstractions" "grep -RIn --include='*.cs' -E 'class [A-Za-z0-9_]*(Reader|Writer|ReadData|WriteData)\\b|interface I[A-Za-z0-9_]*(Reader|Writer|ReadData|WriteData)\\b' src/Modules"
check_absent "Repository abstractions" "grep -RIn --include='*.cs' -E 'interface I[A-Za-z0-9_]*Repository\\b|class [A-Za-z0-9_]*Repository\\b' src/Modules"
check_absent "non-Identity shared I*Query/I*Command" "grep -RIn --include='I*Query.cs' --include='I*Command.cs' -E 'interface I.*(Query|Command)' src/Modules --exclude-dir=Identity"
check_absent "Dapper materializing EF entities outside Identity" "grep -RIn --include='*.cs' -E 'Query(Single|First|SingleOrDefault|FirstOrDefault)?Async<[^>]*Entity>|QueryAsync<[^>]*Entity>' src/Modules --exclude-dir=Identity"
check_absent "Query classes using EF DbContext" "grep -RIn --include='*.cs' -E 'class .*Query\\([^)]*(DbContext|I[A-Za-z0-9]+DbContext)' src/Modules --exclude-dir=Identity"
check_absent "Dapper write execution" "python3 architecture/check_dapper_writes.py"
if [ "$fail" -ne 0 ]; then exit 1; fi
echo 'PASS: strict Vertical Slice read/write compliance gate'
