# SmartSchool Duende IdentityServer v8 - database backed

IdentityServer configuration is no longer in-memory.

## Stores
- `identity`: ASP.NET Core Identity users/roles/claims.
- `identity_server`: Duende ConfigurationDbContext and PersistedGrantDbContext.
- Configuration store: clients, identity resources, API resources/scopes, CORS.
- Operational store: authorization codes, refresh/reference tokens, consents, device/PAR/session/signing-key data supported by Duende v8.

## Provider switching
The Identity module reads the same `Persistence:Provider` and
`Persistence:ConnectionStringName` settings as SmartSchool.

Supported values:
- `PostgreSql`
- `SqlServer`

All three EF contexts use the selected provider:
1. SmartSchoolIdentityDbContext
2. ConfigurationDbContext
3. PersistedGrantDbContext

## Database creation
Use:
- `scripts/identity/build-postgresql.ps1`
- `scripts/identity/build-sqlserver.ps1`

These create provider-specific migrations from the official Duende EF v8 models and
apply all three stores. This avoids maintaining a hand-written copy of Duende's schema.

## Seed
`DuendeConfigurationSeeder` populates:
- openid, profile, email, smartschool.profile
- smartschool.api
- smartschool-api
- smartschool-portal
- smartschool-mobile

No `AddInMemoryClients`, `AddInMemoryApiScopes`, `AddInMemoryApiResources` or
`AddInMemoryIdentityResources` remain.

## Signing
`AddDeveloperSigningCredential` is controlled by
`DuendeIdentityServer:UseDeveloperSigningCredential` and should only be enabled for
development. Production should use persistent signing-key/certificate configuration.
