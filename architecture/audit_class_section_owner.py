from pathlib import Path
import re
import sys

root = Path(__file__).resolve().parents[1]
modules = root / "src" / "Modules"

class_section_entities = []
class_section_mappings = []
retired_section_entities = []

for path in modules.rglob("*.cs"):
    if "Persistence/Migrations/" in path.as_posix():
        continue

    text = path.read_text(encoding="utf-8", errors="ignore")
    if re.search(r"\b(class|record)\s+ClassSectionEntity\b", text):
        class_section_entities.append(path)
    if 'ToTable("class_section"' in text:
        class_section_mappings.append(path)
    if re.search(r"\b(class|record)\s+SectionEntity\b", text):
        retired_section_entities.append(path)

errors = []
if len(class_section_entities) != 1:
    errors.append(f"Expected exactly one ClassSectionEntity declaration, found {len(class_section_entities)}")
if len(class_section_mappings) != 1:
    errors.append(f"Expected exactly one EF mapping to academic.class_section, found {len(class_section_mappings)}")
if retired_section_entities:
    errors.append("Retired SectionEntity still exists: " + ", ".join(str(p.relative_to(root)) for p in retired_section_entities))

expected = Path("src/Modules/Organization/Models/ClassSectionEntity.cs")
if class_section_entities and class_section_entities[0].relative_to(root) != expected:
    errors.append(f"ClassSectionEntity must be owned by Organization: {class_section_entities[0].relative_to(root)}")

if errors:
    print("ClassSection ownership audit FAILED")
    for error in errors:
        print(" -", error)
    sys.exit(1)

print("ClassSection ownership audit PASSED")
