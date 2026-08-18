# SmartSchool configurable data, cache, identity and logging

## Persistence
`Persistence:Provider` accepts:
- `Mock`
- `PostgreSql`
- `SqlServer`

Production and staging default to PostgreSQL. Development defaults to EF InMemory.

For SQL Server use:
`Server=localhost;Database=SmartSchool;Trusted_Connection=True;TrustServerCertificate=True`

## Cache
HybridCache is always the application cache API.
`Caching:Provider` selects the backing distributed cache:
- `Memory`
- `Redis`

This keeps feature handlers independent of the physical cache provider.

## Authentication
`Identity:Provider` accepts:
- `Mock` for development
- `IdentityServer` for OIDC/JWT bearer validation

Mock authentication creates an Administrator principal with the demo tenant claim. It must not be enabled in production.

## Logging
Serilog configuration is externalized in appsettings. Console and rolling-file sinks are enabled. Request correlation and tenant/user enrichment should be added at the API middleware boundary.

## Domain profiles
Detailed profile entities were added for Student, Parent/Guardian, Teacher, Payroll and Driver. Adult CNIC is represented on guardian/teacher/driver profiles. CNIC must be protected as sensitive PII in production: restrict authorization, avoid logs, encrypt/tokenize where appropriate, and never expose it in list DTOs by default.

## Architecture constraints retained
- Modular monolith
- Vertical Slice/CQRS
- mediator
- Result pattern
- feature-local response records
- no Contracts folders
- no generic repositories
- explicit entity-specific Query/Command persistence abstractions
- System.Text.Json
- Central Package Management
- human-readable code and descriptive names
