# SmartSchool Persistence Convention

## No Generic Repository

SmartSchool does **not** use `IRepository<TEntity>`.

Each domain model owns explicit read and write contracts.

Example:

```text
Students/
├── Persistence/
│   ├── IStudentQuery.cs
│   ├── StudentQuery.cs
│   ├── IStudentCommand.cs
│   └── StudentCommand.cs
└── Features/
    └── Student/
        ├── CreateStudent.cs
        ├── GetStudentById.cs
        ├── GetStudentPage.cs
        ├── UpdateStudent.cs
        └── DeleteStudent.cs
```

Handlers depend only on the capability they require.

Read handler:

```csharp
public sealed class Handler(
    IStudentQuery studentQuery)
{
}
```

Write handler:

```csharp
public sealed class Handler(
    IStudentQuery studentQuery,
    IStudentCommand studentCommand,
    IValidator<Request> validator)
{
}
```

`IStudentQuery` owns student reads.

`IStudentCommand` owns student writes.

The same convention is applied to Teacher, Parent, Exam, Fee, Employee,
Payroll, AI execution, and all other module models.

## Why

This avoids the generic-repository abstraction leaking across the domain.

It also allows each query implementation to use the best persistence mechanism:

- EF Core for normal aggregate queries
- Dapper for optimized reports/read models
- PostgreSQL-specific SQL where appropriate
- projections instead of loading complete entities

Command implementations remain focused on aggregate persistence and transaction boundaries.

## Important

The generated concrete `StudentQuery`, `StudentCommand`, and equivalent classes
are persistence integration points. Their methods intentionally throw
`NotImplementedException` until the final module DbContext and database mappings
are connected. They are not fake in-memory repositories.

## Project files

Individual `.csproj` files are intentionally minimal:

```xml
<Project Sdk="Microsoft.NET.Sdk" />
```

The API project uses:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web" />
```

Package versions are centralized in `Directory.Packages.props`.

Shared package references and project references are centralized in
`Directory.Build.targets` using project-name conditions. This keeps module
project files free from repeated package declarations.
