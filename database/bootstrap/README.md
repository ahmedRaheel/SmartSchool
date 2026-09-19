# SmartSchool PostgreSQL bootstrap scripts

These scripts complement the module-owned EF Core migrations. They are intentionally outside any business module because they create shared platform/operational infrastructure or seed reference data.

## Execution order

1. `00_CreatePlatformInfrastructure.PostgreSql.sql`
   - creates the `platform` schema
   - creates `platform.business_number_sequence`
   - required by `IBusinessNumberGenerator` before create operations such as School, Branch, Student, Finance, Payroll, Documents, etc.

2. Apply all module EF Core migrations.

3. `01_CreateObservabilityTables.PostgreSql.sql`
   - creates shared observability/operational tables.

4. `02_SeedLookupAndReferenceData.PostgreSql.sql`
   - populates lookup/reference data after the Reference module tables exist.

## Docker / PowerShell examples

```powershell
Get-Content .\database\bootstrap\00_CreatePlatformInfrastructure.PostgreSql.sql -Raw |
    docker exec -i postgres psql -U postgres -d smartschool_pro

.\scripts\Update-AllModuleDatabases.ps1 -FailAtEnd

Get-Content .\database\bootstrap\01_CreateObservabilityTables.PostgreSql.sql -Raw |
    docker exec -i postgres psql -U postgres -d smartschool_pro

Get-Content .\database\bootstrap\02_SeedLookupAndReferenceData.PostgreSql.sql -Raw |
    docker exec -i postgres psql -U postgres -d smartschool_pro
```

All bootstrap SQL scripts are intended to be idempotent.
