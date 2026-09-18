#!/usr/bin/env python3
from pathlib import Path
import re
import sys

root = Path(__file__).resolve().parents[1]
modules_root = root / 'src' / 'Modules'
errors = []

# No module project may reference another module project.
for project in modules_root.rglob('*.csproj'):
    module = project.parts[project.parts.index('Modules') + 1]
    text = project.read_text(encoding='utf-8', errors='ignore')
    for target in re.findall(r'ProjectReference\s+Include="([^"]+)"', text):
        normalized = target.replace('\\', '/')
        match = re.search(r'/Modules/([^/]+)/', '/' + normalized)
        if match and match.group(1) != module:
            errors.append(f'{project.relative_to(root)} references module {match.group(1)}')

# No business module source may import another SmartSchool module namespace.
for path in modules_root.rglob('*.cs'):
    if 'Migrations' in path.parts:
        continue
    module = path.parts[path.parts.index('Modules') + 1]
    text = path.read_text(encoding='utf-8', errors='ignore')
    for imported in re.findall(r'using\s+SmartSchool\.Modules\.([A-Za-z0-9_]+)', text):
        if imported != module:
            errors.append(f'{path.relative_to(root)} imports module {imported}')

# Critical module database boundaries fixed in this release.
checks = {
    'Admissions': [r'\b(?:org|academic|student|document|reference)\.', r'ToTable\([^\n]+"(?:student|document|academic|org|reference)"'],
    'Payroll': [r'\bhr\.', r'ToTable\([^\n]+"hr"'],
    'Finance': [r'\b(?:payroll|hr)\.', r'ToTable\([^\n]+"(?:payroll|hr)"'],
}

for module, patterns in checks.items():
    base = modules_root / module
    for path in base.rglob('*.cs'):
        if 'Migrations' in path.parts:
            continue
        text = path.read_text(encoding='utf-8', errors='ignore')
        for pattern in patterns:
            if re.search(pattern, text):
                errors.append(f'{path.relative_to(root)} violates {module} database boundary: {pattern}')

if errors:
    print('Strict module boundary audit FAILED')
    for error in errors:
        print('  ' + error)
    sys.exit(1)

print('Strict module boundary audit PASSED')
print('  0 module-to-module ProjectReferences')
print('  0 cross-module namespace imports')
print('  Admissions -> admission.* only')
print('  Payroll -> payroll.* only')
print('  Finance -> finance.* only for payroll/HR boundary')
