# SmartSchool runtime + role + audit fixes

- Added canonical `Role : short` enum: SuperAdmin, Tenant, Principal, Admin, Teacher, Student, Parent, Driver, Accountant, HRManager, Librarian, Examiner.
- Identity role seeding now comes from the enum.
- New tenant owner account is provisioned as `Role.Tenant` (account type and assigned role), not Admin.
- Kept legacy SmartSchoolRoles constants only as enum-backed compatibility aliases so existing authorization code continues compiling.
- Added automatic EF Core audit capture for Added/Modified/Deleted tenant-owned entities. Audit records include tenant, actor user when available, action, entity type/id, before/after JSON, IP, correlation id and occurrence time.
- Audit read-model columns/index are aligned so tenant/school/actor/domain activity is visible in Audit Log.
- Fixed runtime failures from the supplied logs: inventory purchase order relation, library loan tenant ownership/read model, HR employee education is_active mismatch, department metadata/read columns, HR leave-request read columns, LMS lesson relation, finance fee structure relation, exam grade scale relation, and student exam result tenant ownership/read model.
- Added `database/20260902_runtime_error_alignment_and_audit.sql`; also appended it to the consolidated v87 SQL for fresh installs.

Build was not executed because the .NET SDK is not installed in this execution environment.
