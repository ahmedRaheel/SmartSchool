# EF Core relational mapping fix

Module projects containing `IEntityTypeConfiguration<TEntity>` now reference
`Microsoft.EntityFrameworkCore.Relational` through Central Package Management.

This enables relational mapping extensions including:

- `builder.ToTable("Table", schema: "Schema")`
- `HasColumnType`
- `HasDefaultValueSql`
- relational constraint naming

Example:

```csharp
builder.ToTable(
    "ParentToolExecutions",
    schema: "AIParent");
```

Provider packages remain in Infrastructure. Module/domain projects depend only on
the provider-neutral EF Core relational mapping package.
