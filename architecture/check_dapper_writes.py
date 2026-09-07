from pathlib import Path
import re
root=Path('src/Modules')
viol=[]
for p in root.rglob('*.cs'):
    if 'Identity' in p.parts: continue
    s=p.read_text(errors='ignore')
    # Only flag write SQL that is executed through Dapper connection APIs, not EF Database.ExecuteSql*.
    for m in re.finditer(r'(?:const\s+string|var|string)\s+(\w+)\s*=\s*(?:\$)?"""(.*?)"""|(?:const\s+string|var|string)\s+(\w+)\s*=\s*"([^"\n]*(?:INSERT\s+INTO|UPDATE\s+|DELETE\s+FROM|MERGE\s+INTO)[^"\n]*)"', s, re.I|re.S):
        name=m.group(1) or m.group(3); body=m.group(2) or m.group(4) or ''
        if not re.search(r'\b(INSERT\s+INTO|UPDATE\s+|DELETE\s+FROM|MERGE\s+INTO)\b',body,re.I): continue
        if re.search(rf'\b(?:connection|cn|c)\.Execute(?:Async|ScalarAsync)?[^;]*\b{name}\b',s,re.I|re.S):
            viol.append(str(p)); break
print('\n'.join(sorted(set(viol))))
