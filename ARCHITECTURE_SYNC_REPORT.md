# SmartSchool Feature-Owned Vertical Slice Architecture

Identity is intentionally excluded.

## Enforced rules
- Each CRUD/use-case slice owns Request/Query/Command, Response, Validator, Handler, feature-specific interface, and implementation.
- Create/Update/Delete/state-changing slices use EF Core through `IApplicationDbContext`; Dapper is not used in command slices.
- Read slices use Dapper/SQL and project directly to the slice Response.
- `Features/DataAccess` is prohibited and has been removed.
- Feature persistence implementations are colocated with the owning feature file and named `*Persistence`.
- Shared generic repositories are not used for feature persistence.
- EF entity configuration remains module infrastructure under `Persistence/Configurations`.

## Verification
Static architecture scan excludes Identity and checks command slices for Dapper/IDbConnectionFactory usage and Features/DataAccess directories.
