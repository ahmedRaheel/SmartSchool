from pathlib import Path
import re
import sys

root = Path(__file__).resolve().parents[1]
modules_root = root / "src" / "Modules"
program = (root / "src" / "SmartSchool.Api" / "Program.cs").read_text()
api_project = (root / "src" / "SmartSchool.Api" / "SmartSchool.Api.csproj").read_text()

errors: list[str] = []
checked = 0

for context_file in sorted(modules_root.glob("*/Persistence/*DbContext.cs")):
    module = context_file.parts[-3]
    if module == "Identity":
        continue

    context_text = context_file.read_text()
    context_match = re.search(r"public sealed class\s+(\w+DbContext)", context_text)
    if context_match is None:
        continue

    context_name = context_match.group(1)
    module_file = modules_root / module / "Module.cs"
    module_text = module_file.read_text()

    if "IConfiguration configuration" not in module_text:
        errors.append(f"{module}: registration does not accept IConfiguration")

    if f"AddModuleDbContext<{context_name}," not in module_text:
        errors.append(f"{module}: {context_name} is not module-owned in registration")

    if f"Add{module}Module(builder.Configuration)" not in program:
        errors.append(f"{module}: Program.cs does not pass IConfiguration")

    expected_reference = f"../Modules/{module}/SmartSchool.Modules.{module}.csproj"
    if expected_reference not in api_project:
        errors.append(f"{module}: SmartSchool.Api is missing project reference")

    migration_dir = modules_root / module / "Persistence" / "Migrations" / "PostgreSql"
    if not migration_dir.exists():
        errors.append(f"{module}: module migration directory is missing")

    checked += 1

if (root / "src" / "SmartSchool.Api" / "ModuleDbContexts.cs").exists():
    errors.append("Central SmartSchool.Api/ModuleDbContexts.cs still exists")

if (root / "src" / "BuildingBlocks" / "SmartSchool.Infrastructure" / "Persistence" / "SmartSchoolDbContext.cs").exists():
    errors.append("Global SmartSchoolDbContext still exists")

if errors:
    print("Module DbContext audit FAILED")
    for error in errors:
        print(f"- {error}")
    sys.exit(1)

print(f"Module DbContext audit PASSED ({checked} business modules checked)")
