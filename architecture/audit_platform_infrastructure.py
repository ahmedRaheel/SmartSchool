from pathlib import Path
import sys

root = Path(__file__).resolve().parents[1]
source = root / "src" / "BuildingBlocks" / "SmartSchool.Infrastructure" / "Persistence" / "BusinessNumberGenerator.cs"
bootstrap = root / "database" / "bootstrap" / "00_CreatePlatformInfrastructure.PostgreSql.sql"

errors = []

if not source.exists():
    errors.append(f"Missing generator source: {source}")
if not bootstrap.exists():
    errors.append(f"Missing platform bootstrap script: {bootstrap}")

if source.exists() and bootstrap.exists():
    source_text = source.read_text(encoding="utf-8")
    bootstrap_text = bootstrap.read_text(encoding="utf-8").lower()

    required = "platform.business_number_sequence"
    if required not in source_text:
        errors.append("BusinessNumberGenerator no longer references platform.business_number_sequence; audit needs review.")
    if required not in bootstrap_text:
        errors.append("Platform bootstrap does not create platform.business_number_sequence.")
    if "primary key (tenant_id, sequence_name)" not in " ".join(bootstrap_text.split()):
        # Formatting-insensitive fallback checks below.
        if "tenant_id" not in bootstrap_text or "sequence_name" not in bootstrap_text or "primary key" not in bootstrap_text:
            errors.append("Sequence table must enforce uniqueness for tenant_id + sequence_name.")

if errors:
    print("Platform infrastructure audit FAILED")
    for error in errors:
        print(f" - {error}")
    sys.exit(1)

print("Platform infrastructure audit PASSED")
