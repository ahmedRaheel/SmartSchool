from pathlib import Path
import re, sys
root = Path(__file__).resolve().parents[2] / 'src' / 'Modules'
scopes = ['Students', 'HR', 'Tenancy']
patterns = {
    'database connection in feature': re.compile(r'\bIDbConnectionFactory\b'),
    'Dapper command in feature': re.compile(r'\bCommandDefinition\b|\bQueryAsync\b|\bExecuteAsync\b|\bExecuteScalarAsync\b'),
    'SQL in feature': re.compile(r'\bSELECT\s+.+\s+FROM\b|\bINSERT\s+INTO\b|\bUPDATE\s+[a-zA-Z_]|\bDELETE\s+FROM\b', re.I | re.S),
    'DbContext in feature': re.compile(r'\bIApplicationDbContext\b|\bDbContext\b'),
}
violations=[]
for scope in scopes:
    for path in (root/scope/'Features').rglob('*.cs'):
        text=path.read_text(encoding='utf-8', errors='ignore')
        for name, pat in patterns.items():
            if pat.search(text): violations.append((path.relative_to(root), name))
if violations:
    print('Vertical Slice persistence boundary violations:')
    for p,n in violations: print(f' - {p}: {n}')
    sys.exit(1)
print('OK: Students, HR and Tenancy feature handlers contain no direct database access or SQL.')
