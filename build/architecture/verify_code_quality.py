from pathlib import Path
import re, sys
root=Path(__file__).resolve().parents[2]/'src'
viol=[]
for p in root.rglob('*.cs'):
    s=p.read_text(encoding='utf-8-sig')
    rel=p.relative_to(root.parent)
    if re.search(r'private static Response MapResponse\([^)]*SmartSchool\.Modules\.',s,re.S): viol.append(f'{rel}: fully-qualified MapResponse parameter')
    if p.name!='AuthorizationConstants.cs' and re.search(r'"(?:SuperAdmin|Teacher|Student|Parent|Driver)"',s): viol.append(f'{rel}: magic role string')
    if p.name!='ApplicationConstants.cs' and re.search(r'"(?:ACTIVE|INACTIVE|PENDING|PENDING_APPROVAL|APPROVED|REJECTED|HIRED|SUBMITTED|WAITING_LIST)"',s): viol.append(f'{rel}: magic lifecycle status')
print(f'Code-quality violations: {len(viol)}')
for v in viol: print(v)
sys.exit(1 if viol else 0)
