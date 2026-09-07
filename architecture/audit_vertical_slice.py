from pathlib import Path
import re, sys
root=Path(__file__).resolve().parents[1]/'src'/'Modules'
viol=[]
for p in root.rglob('*.cs'):
    rel=p.relative_to(root).as_posix()
    if rel.startswith('Identity/'):
        continue
    text=p.read_text(errors='ignore')
    if p.name=='Module.cs' and re.search(r'QueryAsync|QuerySingle|ExecuteSql|const string sql|INSERT INTO|UPDATE\s+\w|DELETE FROM', text, re.I):
        viol.append((rel,'business/data access in Module.cs'))
    if '/DataAccess/' in '/'+rel:
        viol.append((rel,'DataAccess folder is forbidden'))
    if re.search(r'ExecuteSql(?:Raw|Interpolated)|ExecuteDelete|ExecuteUpdate', text):
        viol.append((rel,'raw EF SQL write is forbidden'))
    if re.search(r'\b(?:INSERT\s+INTO|UPDATE\s+[\w.\"]+\s+SET|DELETE\s+FROM)\b', text, re.I) and ('Dapper' in text or 'CommandDefinition' in text):
        viol.append((rel,'Dapper write is forbidden'))
    if re.search(r'(Reader|Writer)\.cs$', p.name):
        viol.append((rel,'Reader/Writer abstraction is forbidden'))
for rel,msg in viol:
    print(f'{rel}: {msg}')
print(f'VIOLATIONS={len(viol)}')
sys.exit(1 if viol else 0)
