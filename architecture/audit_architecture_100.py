from pathlib import Path
import re, sys
ROOT=Path(__file__).resolve().parents[1]
viol=[]
for p in (ROOT/'src'/'Modules').rglob('*.cs'):
    rel=p.relative_to(ROOT).as_posix()
    if 'Identity' in rel: continue
    txt=p.read_text(errors='ignore')
    if '/Features/' in rel:
        n=len(re.findall(r'\bIRequestHandler\s*<',txt))
        if n>1: viol.append((rel,f'{n} handlers in one slice'))
    if '/Persistence/' in rel:
        if '/Configurations/' not in rel and not p.name.endswith('DbContext.cs'):
            viol.append((rel,'Persistence contains non-DbContext/non-configuration file'))
        if re.search(r'\b(IDbConnectionFactory|Dapper|QueryAsync|QuerySingle|ExecuteAsync)\b',txt):
            viol.append((rel,'Dapper/read-write data access in module Persistence'))
    if re.search(r'\b(class|interface)\s+\w*(Reader|Writer|Repository)\b',txt):
        viol.append((rel,'Reader/Writer/Repository abstraction'))
    if re.search(r'ExecuteSql(?:Raw|Interpolated)|\b(?:INSERT|UPDATE|DELETE)\s+',txt,re.I):
        # SQL is allowed only in feature read queries if SELECT; writes never raw SQL
        if re.search(r'ExecuteSql(?:Raw|Interpolated)|\b(?:INSERT|UPDATE|DELETE)\s+',txt,re.I):
            viol.append((rel,'raw SQL write'))
for d in (ROOT/'src'/'Modules').rglob('*'):
    if d.is_dir() and d.name.lower() in {'dataaccess','readers','writers'} and 'Identity' not in d.as_posix():
        viol.append((d.relative_to(ROOT).as_posix(),'forbidden folder'))
# duplicate DbContext type/file names per module
for module in (ROOT/'src'/'Modules').iterdir():
    if not module.is_dir() or module.name=='Identity': continue
    dbs=list(module.rglob('*DbContext.cs'))
    names={}
    for p in dbs: names.setdefault(p.name,[]).append(p)
    for name,ps in names.items():
        if len(ps)>1: viol.append((module.name,f'duplicate {name}: {len(ps)}'))
print(f'VIOLATIONS={len(viol)}')
for a,b in viol: print(f'{a}: {b}')
sys.exit(1 if viol else 0)
