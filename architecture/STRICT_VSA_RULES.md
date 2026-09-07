# Strict Vertical Slice Rules

Identity is excluded from this enforcement.

1. One use case owns Request/Query, Response record, Validator, feature-specific interface, implementation and Handler.
2. Reads use Dapper and project directly to feature-owned records/DTOs. Reads do not materialize EF entities.
3. Creates/updates/deletes use EF Core and domain entities/aggregates.
4. No DataAccess, Reader, Writer, ReadData, WriteData, Repository, workflow-wide persistence, or feature-group persistence abstractions.
5. Persistence contains DbContext and EF entity configurations only.
