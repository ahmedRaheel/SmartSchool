from pathlib import Path
root = Path(__file__).resolve().parents[1]
forbidden = [
    "StudentDocumentEntity", "ParentDocumentEntity", "TeacherDocumentEntity",
    "EmployeeDocumentEntity", "CandidateDocumentEntity", "DriverDocumentEntity", "SchoolDocumentEntity"
]
violations = []
for path in (root / "src" / "Modules").rglob("*.cs"):
    if "Modules/Documents" in path.as_posix():
        continue
    text = path.read_text(errors="ignore")
    for token in forbidden:
        if token in text:
            violations.append(f"{path.relative_to(root)}: {token}")
print(f"CENTRAL_DOCUMENT_VIOLATIONS={len(violations)}")
for violation in violations:
    print(violation)
raise SystemExit(1 if violations else 0)
