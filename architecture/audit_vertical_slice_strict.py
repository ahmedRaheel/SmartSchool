#!/usr/bin/env python3
import os,re,sys
ROOT=os.path.join('src','Modules')
CMD=('Create','Update','Delete','Approve','Reject','Archive','Assign','Submit','Complete','Change','Strike','Terminate','Upload','Publish','Grade','Apply','Add','Remove','Record','Mark','Cancel','Close','Open','Generate','Start','Finish','Restore','Set','Save','Process','Import','Onboard','Hire')
QRY=('Get','List','Search','Find','Download','Check','Validate','Resolve','Preview','Export')
viol=[]
for dp,ds,fs in os.walk(ROOT):
    n=dp.replace('\\','/')
    if '/Identity' in n: ds[:]=[]; continue
    ds[:]=[d for d in ds if d not in ('bin','obj')]
    if '/Features/' in n and '/DataAccess' in n: viol.append(f'DataAccess folder: {n}')
    for fn in fs:
        if not fn.endswith('.cs'): continue
        p=os.path.join(dp,fn); pn=p.replace('\\','/'); s=open(p,encoding='utf-8',errors='ignore').read()
        if '/Features/' in pn and re.search(r'(Reader|Writer)\.cs$',fn): viol.append(f'Reader/Writer: {pn}')
        if fn=='Module.cs' and re.search(r'\b(SELECT|INSERT|UPDATE|DELETE)\b|QueryAsync|ExecuteSql|SaveChangesAsync',s,re.I): viol.append(f'Business/data access in Module.cs: {pn}')
        m=re.search(r'public\s+static\s+class\s+(\w+)',s)
        if not m or '/Features/' not in pn: continue
        name=m.group(1); kind='Command' if name.startswith(CMD) else ('Query' if name.startswith(QRY) else None)
        if kind and not re.search(r'interface\s+I'+re.escape(name+kind)+r'\b',s): viol.append(f'Missing feature-owned I{name}{kind}: {pn}')
        if kind and not re.search(r'class\s+'+re.escape(name+kind)+r'\b',s): viol.append(f'Missing feature-owned {name}{kind}: {pn}')
        if kind and not re.search(r'class\s+Handler\b',s): viol.append(f'Missing Handler: {pn}')
        if kind=='Query' and re.search(r'Query(?:Single|First|)Async<\s*\w+Entity\b',s): viol.append(f'Dapper materializes entity: {pn}')
        if re.search(r'ExecuteSql(?:Raw|Interpolated)|\bINSERT\s+INTO\b|\bDELETE\s+FROM\b',s,re.I): viol.append(f'Raw write SQL: {pn}')
print('\n'.join(viol))
print(f'VIOLATIONS={len(viol)}')
sys.exit(1 if viol else 0)
