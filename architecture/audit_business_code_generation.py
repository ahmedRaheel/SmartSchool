from pathlib import Path
import re
root = Path(__file__).resolve().parents[1]
violations=[]
patterns=[
    re.compile(r'Guid\.NewGuid\(\)\.ToString\("N"\)\.ToUpperInvariant\(\)'),
    re.compile(r'\$"[A-Z][A-Z0-9_-]*-\{Guid\.NewGuid\(\):N\}"'),
    re.compile(r'\b(?:code|Code)\s*=.*Guid\.NewGuid\(', re.I),
]
for path in (root/'src/Modules').rglob('*.cs'):
    if '/Identity/' in path.as_posix():
        continue
    text=path.read_text(errors='ignore')
    for n,line in enumerate(text.splitlines(),1):
        if any(p.search(line) for p in patterns):
            violations.append(f'{path.relative_to(root)}:{n}: {line.strip()}')
print(f'BUSINESS_CODE_GUID_VIOLATIONS={len(violations)}')
for v in violations: print(v)
raise SystemExit(1 if violations else 0)
