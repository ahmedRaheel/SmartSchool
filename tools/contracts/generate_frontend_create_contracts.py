from pathlib import Path
import re,json
root=Path('/mnt/data/sync87/backend/SmartSchool/src/Modules')
out=Path('/mnt/data/sync87/frontend/SmartSchool_React/src/core/contracts/createContractRegistry.ts')

def extract(text,name):
 m=re.search(rf'public\s+sealed\s+record\s+{name}\s*\((.*?)\)\s*(?::|;)',text,re.S)
 if not m:return []
 fields=[]
 for raw in re.split(r',\s*(?![^<]*>)',m.group(1)):
  raw=re.sub(r'\s+',' ',raw).strip(); mm=re.match(r'(.+?)\s+(\w+)$',raw)
  if mm: fields.append((mm.group(2),mm.group(1)))
 return fields

def meta(name,t):
 nullable='?' in t; base=t.replace('?','').strip(); lower=name.lower()
 if base=='byte[]': kind='file'
 elif base=='bool': kind='boolean'
 elif base in {'int','long','decimal','double','float'}: kind='number'
 elif base=='DateOnly' or lower.endswith('date'): kind='date'
 elif base in {'DateTime','DateTimeOffset'} or lower.endswith('at'): kind='dateTime'
 else: kind='string'
 return {'name':name[0].lower()+name[1:],'kind':kind,'required':not nullable}
items=[]
for f in root.rglob('Create*.cs'):
 text=f.read_text(errors='ignore'); req=extract(text,'Request'); resp=extract(text,'Response')
 route=re.search(r'EntityCollection\(ModuleConstants\.RouteSegment,\s*"([^"]+)"\)',text)
 if not route or not req: continue
 module=f.relative_to(root).parts[0].lower(); resource=route.group(1)
 items.append({'module':module,'resource':resource,'requestFields':[meta(*x) for x in req],'responseFields':[meta(*x) for x in resp]})
items.sort(key=lambda x:(x['module'],x['resource']))
lines=['/**',' * Generated from backend Create*.Request and Create*.Response records.',' * This registry keeps create panels synchronized with the compiled API contract.',' */','','export type ContractFieldKind = "string" | "number" | "boolean" | "date" | "dateTime" | "file";','','export interface ContractField {','    name: string;','    kind: ContractFieldKind;','    required: boolean;','}','','export interface CreateContract {','    module: string;','    resource: string;','    requestFields: ContractField[];','    responseFields: ContractField[];','}','','export const createContracts: CreateContract[] = [']
for x in items:
 lines.append('    '+json.dumps(x,separators=(',',':'))+',')
lines += ['];','','export function findCreateContract(module: string, resource: string): CreateContract | undefined {','    return createContracts.find(','        contract => contract.module === module.toLowerCase() && contract.resource === resource,','    );','}','']
out.write_text('\n'.join(lines)); print(len(items))
