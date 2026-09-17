from pathlib import Path
import re
import sys
from collections import Counter

root = Path(__file__).resolve().parents[1]
seed_file = root / "database" / "bootstrap" / "02_SeedLookupAndReferenceData.PostgreSql.sql"
text = seed_file.read_text(encoding="utf-8")
head = text.split("-- -------------------------------------------------------------------------\n-- Lookup values", 1)[0]
lookup_types = set(re.findall(r"\('([A-Z0-9_]+)'\s*,\s*'[^']*'\s*,\s*(?:TRUE|FALSE)\)", head))
pairs = re.findall(r"\('([A-Z0-9_]+)'\s*,\s*'([A-Z0-9_]+)'\s*,", text)
seeded_types = {type_code for type_code, _ in pairs}
duplicates = [pair for pair, count in Counter(pairs).items() if count > 1]
missing = sorted(lookup_types - seeded_types)

errors = []
if missing:
    errors.append("Lookup types without values: " + ", ".join(missing))
if duplicates:
    errors.append("Duplicate lookup values: " + ", ".join(f"{a}/{b}" for a, b in duplicates))
for table in (
    "saas.lookup_type",
    "saas.lookup_value",
    "reference.branch_gender_type",
    "reference.education_level",
    "reference.country",
    "reference.province",
    "reference.city",
):
    if table not in text:
        errors.append(f"Reference seed does not cover {table}")

if errors:
    print("Reference seed audit FAILED")
    for error in errors:
        print(" -", error)
    sys.exit(1)

print(f"Reference seed audit PASSED ({len(lookup_types)} lookup types covered)")
