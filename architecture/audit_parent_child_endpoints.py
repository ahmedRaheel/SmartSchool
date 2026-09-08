#!/usr/bin/env python3
import pathlib,re,sys
root=pathlib.Path(__file__).resolve().parents[1]
ddl=(root/'database/SmartSchoolComplete.Consolidated.sql').read_text(errors='ignore')
fkpat=re.compile(r'ALTER TABLE ONLY\s+([\w]+)\.([\w]+)\s+ADD CONSTRAINT\s+\S+\s+FOREIGN KEY \((\w+)\) REFERENCES\s+([\w]+)\.([\w]+)\((\w+)\)',re.I|re.S)
def pascal(s): return ''.join(x[:1].upper()+x[1:] for x in s.split('_'))
byid={}
for p in root.glob('src/Modules/**/Features/**/Get*ById.cs'):
    if '/Identity/' in str(p): continue
    text=p.read_text(errors='ignore')
    m=re.search(r'FROM\s+([\w]+)\.([\w]+)\s+AS\s+entity',text,re.I)
    if m: byid[(m.group(1),m.group(2))]=p
viol=[]; checked=0
for m in fkpat.finditer(ddl):
    schema,table,column,*_=m.groups()
    if column=='tenant_id' or (schema,table) not in byid: continue
    checked+=1; folder=byid[(schema,table)].parent
    if not list(folder.glob(f'Get*By{pascal(column)}.cs')):
        viol.append(f'{schema}.{table}.{column}')
print(f'API_BACKED_PARENT_RELATIONSHIPS={checked}')
print(f'VIOLATIONS={len(viol)}')
for v in viol: print(v)
sys.exit(1 if viol else 0)
