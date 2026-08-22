# Database providers and identity

## EF Core providers

`SmartSchool.Infrastructure` owns the physical database provider packages:

- `Npgsql.EntityFrameworkCore.PostgreSQL`
- `Microsoft.EntityFrameworkCore.SqlServer`

Therefore these compile:

```csharp
dbContextOptions.UseNpgsql(connectionString);
dbContextOptions.UseSqlServer(connectionString);
```

Module projects continue to depend on `Microsoft.EntityFrameworkCore.Relational`
for provider-neutral mappings such as `ToTable`.

## IdentityServer4

The requested IdentityServer4 4.1.2 host is isolated under
`src/Identity/SmartSchool.IdentityServer4` and targets `netcoreapp3.1`.

It must not be pulled into the .NET 10 API dependency graph. The API consumes
OIDC/JWT tokens through its configured Authority and Audience.
