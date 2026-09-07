from pathlib import Path
import re, sys
root=Path('src/Modules')
viol=[]
for p in root.rglob('*.cs'):
    if 'Identity' in p.parts: continue
    text=p.read_text(errors='ignore')
    if any(x.lower()=='dataaccess' for x in p.parts): viol.append(f'DataAccess folder: {p}')
    for m in re.finditer(r'(?s)(?:public|internal)\s+interface\s+(I\w+)\s*\{(.*?)\n\s*\}', text):
        refs=set(re.findall(r'([A-Z]\w+)\.Request',m.group(2)))
        if len(refs)>1: viol.append(f'Cross-feature interface {m.group(1)} in {p}: {sorted(refs)}')
    if re.search(r'\b(?:I?\w*Workflow(?:Command|Query)|I?\w*WorkflowSlices\w*(?:Command|Query))\b', text):
        viol.append(f'Grouped workflow command/query abstraction: {p}')
if viol:
    print('\n'.join(viol)); sys.exit(1)
print('PASS: no DataAccess folders, no cross-feature interfaces, no grouped workflow command/query abstractions outside Identity')
