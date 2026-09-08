#!/usr/bin/env python3
from pathlib import Path
import re
root = Path(__file__).resolve().parents[1] / 'src' / 'Modules'
violations = []
for path in root.rglob('*.cs'):
    if 'Identity' in path.parts:
        continue
    text = path.read_text(encoding='utf-8', errors='ignore')
    handlers = len(re.findall(r'\bIRequestHandler\s*<', text))
    if handlers > 1:
        violations.append((path, handlers))
if violations:
    for path, count in violations:
        print(f'{path}: {count} IRequestHandler implementations')
    raise SystemExit(1)
print('VIOLATIONS=0')
