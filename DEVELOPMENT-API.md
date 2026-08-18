# SmartSchool Development API

## OpenAPI and Scalar

In Development the API exposes:

- OpenAPI JSON: `/openapi/v1.json`
- Scalar API Reference: `/scalar/v1`

Scalar is backed by ASP.NET Core's first-party OpenAPI generator.

## EF Core mock database

Development persistence uses `Microsoft.EntityFrameworkCore.InMemory` through `SmartSchoolMockDbContext`.
The existing module-specific interfaces and classes (`IStudentQuery`, `StudentQuery`, `IStudentCommand`, `StudentCommand`, etc.) are preserved. They delegate to the shared EF-backed development store, so the Vertical Slice handlers remain unchanged.

All 232 scaffolded persistence implementations are connected. CRUD, duplicate-code checks, tenant filtering and paging execute against EF Core rather than throwing `NotImplementedException`.

At development startup `MockDatabaseSeeder` creates three records for every mapped SmartSchool entity using demo tenant:

`11111111-1111-1111-1111-111111111111`

The in-memory database lives for the lifetime of the API process and resets when the process restarts. It is intentionally a development adapter and can later be replaced by PostgreSQL/SQL Server module persistence without changing feature handlers.
