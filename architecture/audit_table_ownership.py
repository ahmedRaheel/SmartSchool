#!/usr/bin/env python3
from pathlib import Path
import re
import sys
from collections import defaultdict

root = Path(__file__).resolve().parents[1]
modules = root / 'src' / 'Modules'
pattern = re.compile(r'ToTable\(\s*"([^"]+)"\s*,\s*(?:schema:\s*)?"([^"]+)"')
owners = defaultdict(set)
locations = defaultdict(list)

for path in modules.rglob('*.cs'):
    if 'Migrations' in path.parts:
        continue
    text = path.read_text(encoding='utf-8', errors='ignore')
    for table, schema in pattern.findall(text):
        module = path.parts[path.parts.index('Modules') + 1]
        key = (schema, table)
        owners[key].add(module)
        locations[key].append(path.relative_to(root))

errors = []
for key, module_names in sorted(owners.items()):
    if len(module_names) > 1:
        errors.append((key, sorted(module_names), locations[key]))

expected = {
    ('academic', 'class_section'): {'Organization'},
    ('document', 'document'): {'Documents'},
    ('student', 'student'): {'Students'},
    ('student', 'guardian'): {'Students'},
    ('student', 'student_guardian'): {'Students'},
    ('student', 'student_enrollment'): {'Students'},
    ('payroll', 'employee_compensation'): {'Payroll'},
}

for key, expected_owner in expected.items():
    actual = owners.get(key, set())
    if actual != expected_owner:
        errors.append((key, sorted(actual), locations.get(key, [])))

if errors:
    print('Table ownership audit FAILED')
    for key, module_names, paths in errors:
        print(f'  {key[0]}.{key[1]} -> {", ".join(module_names) or "<none>"}')
        for path in paths:
            print(f'    {path}')
    sys.exit(1)

print('Table ownership audit PASSED')
print('  academic.class_section -> Organization')
print('  document.document -> Documents')
print('  student admission tables -> Students')
print('  payroll.employee_compensation -> Payroll')
