# SmartSchool Database Scripts

## Structure

- `postgresql/001_schema.sql` — lookup/reference DDL.
- `postgresql/002_lookup_seed.sql` — idempotent reference data.
- `postgresql/000_run_all.sql` — psql runner.
- `sqlserver/001_schema.sql` — SQL Server lookup/reference DDL.
- `sqlserver/002_lookup_seed.sql` — idempotent reference data.
- `sqlserver/000_run_all.sql` — SQLCMD/SSMS runner.

The existing root `database.sql` remains the broader SmartSchool PostgreSQL domain/document schema. These new folders separate provider-specific deployment scripts and lookup seed data.

Seeded reference domains include occupation, relationship, gender, blood group, employment status/type, marital status, payment method, fee status, attendance status, exam type, document type, vehicle type, and driving-license category.

Reference data uses stable GUIDs and stable codes. Application code should persist lookup IDs/codes rather than free-text values. Seeds are idempotent and can safely be rerun.
