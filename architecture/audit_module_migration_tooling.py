from pathlib import Path
import re

root = Path(__file__).resolve().parents[1]
modules_root = root / "src" / "Modules"
expected = {}

for context_file in sorted(modules_root.glob("*/Persistence/*DbContext.cs")):
    module = context_file.parts[-3]
    if module == "Identity":
        continue
    match = re.search(r"public\s+sealed\s+class\s+(\w+DbContext)|public\s+class\s+(\w+DbContext)", context_file.read_text(encoding="utf-8"))
    if match:
        expected[module] = match.group(1) or match.group(2)

required_modules = {
    "AICore", "AIInquiry", "AIParent", "AIPrediction", "AITutor",
    "Activities", "Admissions", "Audit", "Communication", "Documents",
    "Examinations", "Finance", "HR", "Inventory", "Learning", "Library",
    "Organization", "Payroll", "Reference", "Students", "Transport", "Workflow",
}

errors = []
if set(expected) != required_modules:
    errors.append(f"DbContext module set mismatch. Found: {sorted(expected)}")

for module in sorted(required_modules):
    migration_dir = modules_root / module / "Persistence" / "Migrations" / "PostgreSql"
    if not migration_dir.is_dir():
        errors.append(f"Missing migration directory: {migration_dir.relative_to(root)}")

config_script = root / "scripts" / "ModuleMigrationConfig.ps1"
if not config_script.is_file():
    errors.append("Missing migration configuration: scripts/ModuleMigrationConfig.ps1")
else:
    config_text = config_script.read_text(encoding="utf-8")
    for module, context in sorted(expected.items()):
        if f'Name = "{module}"' not in config_text or f'Context = "{context}"' not in config_text:
            errors.append(f"ModuleMigrationConfig.ps1 does not map {module} to {context}")

for script_name in (
    "Add-ModuleMigration.ps1",
    "Add-AllModuleMigrations.ps1",
    "Update-ModuleDatabase.ps1",
    "Update-AllModuleDatabases.ps1",
    "Initialize-AllModuleMigrations.ps1",
    "add-module-migration.sh",
    "add-all-module-migrations.sh",
    "update-module-database.sh",
    "update-all-module-databases.sh",
):
    script = root / "scripts" / script_name
    if not script.is_file():
        errors.append(f"Missing migration helper: scripts/{script_name}")

if errors:
    print("Module migration tooling audit FAILED")
    for error in errors:
        print(f" - {error}")
    raise SystemExit(1)

print(f"Module migration tooling audit PASSED ({len(required_modules)} business modules covered)")
