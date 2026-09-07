from pathlib import Path
import re, sys
root=Path('src/Modules')
viol=[]
for p in root.rglob('*.cs'):
 s=str(p).replace('\\','/')
 if '/Identity/' in s: continue
 text=p.read_text(errors='ignore')
 if '/DataAccess/' in s or re.search(r'\b(?:Reader|Writer|ReadData|WriteData|Repository)\b', p.stem): viol.append((s,'forbidden abstraction/file'))
 if re.search(r'class\s+\w*Query\b[^\{]*(?:I\w*DbContext|DbContext)',text,re.S): viol.append((s,'query depends on EF DbContext'))
 if 'using Dapper;' in text and re.search(r'Query(?:Single|First|SingleOrDefault|FirstOrDefault)?Async<\s*\w+Entity\s*>',text): viol.append((s,'Dapper materializes Entity'))
 if 'using Dapper;' in text and re.search(r'ExecuteAsync\s*\([^;]*(?:INSERT|UPDATE|DELETE|MERGE)',text,re.I|re.S): viol.append((s,'Dapper write'))
for v in viol: print(f'FAIL: {v[0]} :: {v[1]}')
print(f'Violations: {len(viol)}')
sys.exit(1 if viol else 0)
