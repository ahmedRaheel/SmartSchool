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

for script_name in (
    "Add-ModuleMigration.ps1",
    "Add-AllModuleMigrations.ps1",
    "Update-ModuleDatabase.ps1",
    "Update-AllModuleDatabases.ps1",
    "add-module-migration.sh",
    "add-all-module-migrations.sh",
    "update-module-database.sh",
    "update-all-module-databases.sh",
):
    script = root / "scripts" / script_name
    if not script.is_file():
        errors.append(f"Missing migration helper: scripts/{script_name}")
        continue
    text = script.read_text(encoding="utf-8")
    shared_config_text = (root / "scripts" / "ModuleMigrationConfig.ps1").read_text(encoding="utf-8") if (root / "scripts" / "ModuleMigrationConfig.ps1").is_file() else ""
    for module in required_modules:
        covered_by_shared_config = "ModuleMigrationConfig.ps1" in text and module in shared_config_text
        if module not in text and not covered_by_shared_config and "AllModule" not in script_name and "all-module" not in script_name:
            errors.append(f"scripts/{script_name} does not cover {module}")

if errors:
    print("Module migration tooling audit FAILED")
    for error in errors:
        print(f" - {error}")
    raise SystemExit(1)

print(f"Module migration tooling audit PASSED ({len(required_modules)} business modules covered)")
