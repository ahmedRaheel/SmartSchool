#!/usr/bin/env python3
"""Verify that Dapper SQL references canonical schema-qualified PostgreSQL tables."""
from __future__ import annotations

import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
DATABASE_SCRIPT = ROOT / "database" / "postgresql" / "SmartSchoolComplete.sql"
if not DATABASE_SCRIPT.exists():
    DATABASE_SCRIPT = Path("/mnt/data/SmartSchoolComplete(1).sql")

CREATE_TABLE = re.compile(
    r"CREATE\s+TABLE\s+(?:(?:IF\s+NOT\s+EXISTS)\s+)?"
    r"(?:(?:\"([^\"]+)\"|([A-Za-z_]\w*))\.)?"
    r"(?:\"([^\"]+)\"|([A-Za-z_]\w*))\s*\(",
    re.IGNORECASE,
)
TABLE_REFERENCE = re.compile(
    r"\b(?:FROM|JOIN|UPDATE|INTO|DELETE\s+FROM)\s+"
    r"(?:(\"?[A-Za-z_]\w*\"?)\.)?\"?([A-Za-z_]\w*)\"?",
    re.IGNORECASE,
)
CTE_NAMES = {"exams", "assignment_stats", "fee_stats", "payroll_history", "historical", "set"}


def load_tables() -> set[tuple[str, str]]:
    sql = DATABASE_SCRIPT.read_text(encoding="utf-8", errors="ignore")
    tables: set[tuple[str, str]] = set()
    for match in CREATE_TABLE.finditer(sql):
        schema = (match.group(1) or match.group(2) or "public").lower()
        table = (match.group(3) or match.group(4)).lower()
        tables.add((schema, table))
    return tables


def main() -> int:
    tables = load_tables()
    errors: list[str] = []
    references = 0

    for source in (ROOT / "src").rglob("*.cs"):
        text = source.read_text(encoding="utf-8", errors="ignore")
        if "using Dapper;" not in text and "CommandDefinition" not in text:
            continue

        for match in TABLE_REFERENCE.finditer(text):
            schema = (match.group(1) or "").strip('"').lower()
            table = match.group(2).lower()
            references += 1

            if not schema:
                if table not in CTE_NAMES:
                    errors.append(f"{source.relative_to(ROOT)}: unqualified table '{table}'")
                continue

            if schema == "public":
                errors.append(f"{source.relative_to(ROOT)}: public schema reference '{schema}.{table}'")
                continue

            if schema != "excluded" and (schema, table) not in tables:
                errors.append(f"{source.relative_to(ROOT)}: table not in database contract '{schema}.{table}'")

    print(f"Checked {references} Dapper SQL table references against {DATABASE_SCRIPT.name}.")
    if errors:
        print(f"FAILED: {len(errors)} invalid table references")
        for error in errors:
            print(f" - {error}")
        return 1

    print("PASS: every physical Dapper table reference is schema-qualified and exists in the database contract.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
