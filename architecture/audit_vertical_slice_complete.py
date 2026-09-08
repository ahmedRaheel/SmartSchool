#!/usr/bin/env python3
from pathlib import Path
import re, sys
root=Path('src/Modules'); violations=[]
for p in root.rglob('*.cs'):
    parts=set(p.parts)
    if 'Identity' in parts or 'bin' in parts or 'obj' in parts: continue
    text=p.read_text(encoding='utf-8',errors='ignore')
    posix=p.as_posix()
    if '/Features/' in posix:
        handlers=len(re.findall(r'\b(?:class|record)\s+Handler\b',text))
        mediator_handlers=len(re.findall(r'IRequestHandler\s*<',text))
        requests=len(re.findall(r'\bIRequest\s*<',text))
        endpoints=len(re.findall(r'\.Map(?:Get|Post|Put|Delete|Patch)\s*\(',text))
        if handlers>1 or mediator_handlers>1: violations.append(f'Multiple handlers ({max(handlers,mediator_handlers)}): {posix}')
        if requests>1: violations.append(f'Multiple CQRS requests ({requests}): {posix}')
        if endpoints>1: violations.append(f'Multiple HTTP endpoints ({endpoints}): {posix}')
        if re.search(r'(Reader|Writer)\.cs$',p.name): violations.append(f'Reader/Writer file: {posix}')
        if '/DataAccess/' in posix: violations.append(f'DataAccess folder: {posix}')
        if re.match(r'I.*(?:Query|Command)\.cs$', p.name): violations.append(f'Standalone Query/Command interface: {posix}')
        if re.search(r'ExecuteSql(?:Raw|Interpolated)|\bINSERT\s+INTO\b|\bDELETE\s+FROM\b|\bUPDATE\s+[a-zA-Z_][\w.]*\s+SET\b',text,re.I): violations.append(f'Raw write SQL: {posix}')
    if p.name=='Module.cs' and re.search(r'\bSELECT\b|\bINSERT\b|\bDELETE\b|\bUPDATE\b|QueryAsync|ExecuteSql|SaveChangesAsync',text,re.I): violations.append(f'Data/business logic in Module.cs: {posix}')
for d in root.rglob('DataAccess'):
    if 'Identity' not in d.parts: violations.append(f'DataAccess directory: {d.as_posix()}')
print('\n'.join(dict.fromkeys(violations))); print(f'VIOLATIONS={len(dict.fromkeys(violations))}'); sys.exit(1 if violations else 0)
