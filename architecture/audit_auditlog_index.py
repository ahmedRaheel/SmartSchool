from pathlib import Path

root = Path(__file__).resolve().parents[1]
config = root / 'src/Modules/Audit/Persistence/Configurations/AuditLogEntityConfiguration.cs'
interceptor = root / 'src/BuildingBlocks/SmartSchool.Infrastructure/Persistence/AuditSaveChangesInterceptor.cs'

cfg = config.read_text(encoding='utf-8')
code = interceptor.read_text(encoding='utf-8')

errors = []
if 'HasIndex(entity => new { entity.TenantId, entity.Code })\n            .IsUnique()' in cfg:
    errors.append('Audit (TenantId, Code) index must not be unique.')
if 'Add(command, "@code", $"{entityType}.{pending.Action}")' not in code:
    errors.append('Audit interceptor code classification changed unexpectedly.')

if errors:
    print('Audit log index audit FAILED')
    for e in errors:
        print(f' - {e}')
    raise SystemExit(1)

print('Audit log index audit PASSED')
