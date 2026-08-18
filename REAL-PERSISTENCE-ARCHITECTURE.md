# Real persistence architecture

This revision removes `IEfMockStore` from module Query/Command implementations.

## Runtime provider selection

All providers use the same `ApplicationDbContext` and the same module-specific
`StudentQuery`, `StudentCommand`, etc.

`Persistence:Provider` controls only EF Core provider registration:

- `Mock` -> EF Core InMemory
- `PostgreSql` -> Npgsql
- `SqlServer` -> Microsoft SQL Server

Therefore switching provider does not change feature or persistence classes.

## Query best practices

Queries now:
- inject `IApplicationDbContext`
- use `AsNoTracking()`
- scope every read by `TenantId`
- use async EF Core operators
- paginate in SQL
- execute existence checks in the database

## Command best practices

Commands now:
- inject `IApplicationDbContext`
- work with typed `DbSet<TEntity>`
- save through the unit of work
- accept cancellation tokens

## Entity configurations

Generated 121 individual `IEntityTypeConfiguration<TEntity>` classes.
`ApplicationDbContext.OnModelCreating` discovers module assemblies and applies
their configurations using `ApplyConfigurationsFromAssembly`.

Configurations define table, key, tenant index, concurrency token and common
Code/Name relational rules where those properties exist.

## Connection strings

The connection string name comes from `PersistenceOptions.ConnectionStringName`.
No connection string is hard-coded into `ApplicationDbContext`.
