#!/usr/bin/env python3
from __future__ import annotations

import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
MODULES = ROOT / "src" / "Modules"

errors: list[str] = []


def fail(message: str) -> None:
    errors.append(message)


# 1. Direct Dapper page projections must expose every positional Response record member.
for path in MODULES.rglob("Get*Page.cs"):
    text = path.read_text(encoding="utf-8", errors="ignore")

    if "QueryAsync<Response>" not in text:
        continue

    response_match = re.search(
        r"public sealed record Response\s*\((.*?)\);",
        text,
        re.DOTALL,
    )
    page_sql_match = re.search(
        r'const string pageSql\s*=\s*"""(.*?)""";',
        text,
        re.DOTALL,
    )

    if not response_match or not page_sql_match:
        continue

    response_members: list[str] = []

    for part in response_match.group(1).split(","):
        member_match = re.search(
            r"([A-Za-z_]\w*)\s*(?:=.*)?$",
            part.strip(),
        )
        if member_match:
            response_members.append(member_match.group(1))

    aliases = set(
        re.findall(
            r'AS\s+"([^"]+)"',
            page_sql_match.group(1),
            re.IGNORECASE,
        )
    )

    missing = [
        member
        for member in response_members
        if member not in aliases
    ]

    if missing:
        relative = path.relative_to(ROOT)
        fail(
            f"{relative}: page SQL is missing Response aliases: "
            + ", ".join(missing)
        )


# 2. Subject page contract.
subject_page = (
    MODULES
    / "Organization"
    / "Features"
    / "Subject"
    / "GetSubjectPage.cs"
)
subject_text = subject_page.read_text(encoding="utf-8")

for alias in (
    "DepartmentId",
    "DepartmentCode",
    "DepartmentName",
    "CampusId",
    "CampusCode",
    "CampusName",
):
    if f'AS "{alias}"' not in subject_text:
        fail(f"{subject_page.relative_to(ROOT)}: missing {alias} projection.")


# 3. Admissions criteria must not order by a non-existent created_at column.
criteria_file = (
    MODULES
    / "Admissions"
    / "Features"
    / "AdmissionWorkflow"
    / "GetAdmissionCriteria"
    / "GetAdmissionCriteria.cs"
)
criteria_text = criteria_file.read_text(encoding="utf-8")

if re.search(
    r"FROM\s+admission\.admission_criteria.*?ORDER\s+BY\s+created_at",
    criteria_text,
    re.IGNORECASE | re.DOTALL,
):
    fail(
        f"{criteria_file.relative_to(ROOT)}: admission_criteria "
        "orders by created_at, but the model has no created_at column."
    )


# 4. Organization must own org.room because ClassSection/Timetable query it.
room_entity = MODULES / "Organization" / "Models" / "RoomEntity.cs"
room_config = (
    MODULES
    / "Organization"
    / "Persistence"
    / "Configurations"
    / "RoomEntityConfiguration.cs"
)

if not room_entity.exists():
    fail("Organization is missing Models/RoomEntity.cs.")

if not room_config.exists():
    fail("Organization is missing RoomEntityConfiguration.cs.")
else:
    room_config_text = room_config.read_text(encoding="utf-8")
    if 'ToTable("room", "org")' not in room_config_text:
        fail("RoomEntityConfiguration does not map to org.room.")

organization_migrations = list(
    (
        MODULES
        / "Organization"
        / "Persistence"
        / "Migrations"
        / "PostgreSql"
    ).glob("*.cs")
)

if not any(
    'name: "room"' in path.read_text(encoding="utf-8", errors="ignore")
    and 'schema: "org"' in path.read_text(encoding="utf-8", errors="ignore")
    for path in organization_migrations
    if not path.name.endswith(".Designer.cs")
    and "ModelSnapshot" not in path.name
):
    fail("Organization migrations do not create org.room.")


# 5. Inventory canonical naming and JSON column contracts.
purchase_config = (
    MODULES
    / "Inventory"
    / "Persistence"
    / "Configurations"
    / "PurchaseOrderEntityConfiguration.cs"
)
purchase_config_text = purchase_config.read_text(encoding="utf-8")

if (
    'Property(entity => entity.MetadataJson)'
    not in purchase_config_text
    or 'HasColumnName("metadata_json")'
    not in purchase_config_text
):
    fail(
        "PurchaseOrderEntityConfiguration must map MetadataJson "
        "to inventory.purchase_order.metadata_json."
    )

stock_config = (
    MODULES
    / "Inventory"
    / "Persistence"
    / "Configurations"
    / "StockTransactionEntityConfiguration.cs"
)
stock_config_text = stock_config.read_text(encoding="utf-8")

if 'ToTable("stock_transaction", schema: "inventory")' not in stock_config_text:
    fail(
        "StockTransactionEntityConfiguration must use "
        "inventory.stock_transaction."
    )

if (
    'Property(entity => entity.MetadataJson)'
    not in stock_config_text
    or 'HasColumnName("metadata_json")'
    not in stock_config_text
):
    fail(
        "StockTransactionEntityConfiguration must map MetadataJson "
        "to metadata_json."
    )


if errors:
    print("Runtime contract audit FAILED")
    print()

    for error in errors:
        print(f"- {error}")

    sys.exit(1)

print("Runtime contract audit PASSED")
print("Validated:")
print("- Dapper Get*Page Response projections")
print("- Subject page relational projection")
print("- AdmissionCriteria ordering/filter contract")
print("- Organization-owned org.room persistence")
print("- Inventory purchase_order/stock_transaction naming")
