# SmartSchool Persistence Policy

## Read side
Dapper is used for public read APIs, paging, searching, dashboards, reporting and read models.

Every Dapper query:
- owns explicit SQL;
- selects only required columns;
- maps directly to its read response;
- does not inspect EF metadata;
- does not use reflection;
- does not use SELECT *;
- excludes BLOB/document/image columns unless explicitly required.

## Command side
EF Core and SmartSchoolDbContext are used for:
- create;
- update;
- delete/deactivate;
- aggregate loading for domain behavior;
- transactions;
- optimistic concurrency.

Internal GetById/GetByCode may use EF when a command needs the aggregate.
Simple uniqueness/existence checks should prefer AnyAsync rather than loading a full entity.

## Mapping
C# keeps domain-friendly names such as Id and TenantId.
PostgreSQL keeps database-friendly names such as student_id and tenant_id.
EF configurations explicitly bridge these names on the write side.
Dapper SQL uses the physical database names directly on the read side.
