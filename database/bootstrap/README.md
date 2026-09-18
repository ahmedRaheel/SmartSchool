# SmartSchool PostgreSQL Bootstrap

These scripts complement module-owned EF Core migrations.

1. Run module migrations first (`ReferenceDbContext`, `OrganizationDbContext`, etc.).
2. Run `01_CreateObservabilityTables.PostgreSql.sql` to create/upgrade the cross-cutting `observability.application_log` table used by API and Identity Serilog/operations endpoints.
3. Run `02_SeedLookupAndReferenceData.PostgreSql.sql` to populate lookup types/values plus reference master data (branch gender policies, education levels, Pakistan/province/city baseline).

Both scripts are idempotent and safe to rerun. They do not replace EF migrations for module-owned tables.
