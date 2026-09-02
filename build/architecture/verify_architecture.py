from pathlib import Path
import re, sys
root=Path(__file__).resolve().parents[2]
violations=[]
for p in root.glob('src/Modules/*/Features/**/*.cs'):
    t=p.read_text(errors='ignore')
    if re.search(r'(?im)^\s*(SELECT\s+|INSERT\s+INTO\s+|UPDATE\s+[a-z_][\w.]*\s+SET\s+|DELETE\s+FROM\s+)', t): violations.append((p,'LEGACY_SQL_IN_FEATURE_DISABLED'))
    if 'IDbConnectionFactory' in t: violations.append((p,'LEGACY_CONNECTION_IN_FEATURE_DISABLED'))
    if re.search(r'\bIApplicationDbContext\b|\bDbContext\b',t): violations.append((p,'LEGACY_DBCONTEXT_IN_FEATURE_DISABLED'))
for p in root.glob('src/**/*.cs'):
    t=p.read_text(errors='ignore')
    if 'new HttpClient(' in t: violations.append((p,'direct HttpClient construction'))
    if re.search(r'Newtonsoft|JsonConvert|JObject|JToken',t): violations.append((p,'Newtonsoft.Json usage'))
for p in root.glob('src/Modules/*/Persistence/**/*Query.cs'):
    t=p.read_text(errors='ignore')
    if re.search(r'IApplicationDbContext|DbContext|AsNoTracking|EntityFrameworkCore',t): violations.append((p,'EF Core used by Query; reads must use Dapper'))
for p in root.glob('src/Modules/*/Persistence/**/*Command.cs'):
    t=p.read_text(errors='ignore')
    if re.search(r'IDbConnectionFactory|\bDapper\b|QueryAsync|ExecuteAsync',t): violations.append((p,'Dapper used by Command; writes must use EF Core'))
if violations:
    print(f'Architecture violations: {len(violations)}')
    for p,msg in violations: print(f'{p.relative_to(root)}: {msg}')
    sys.exit(1)
print('Architecture verification passed.')
