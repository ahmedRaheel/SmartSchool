# SmartSchool Persistence Read/Write Policy

## Decision

SmartSchool uses **EF Core for writes and aggregate loading** and **Dapper for API/list/search reads**.

### EF Core is allowed for
- POST/create commands.
- PUT/PATCH/update commands.
- DELETE/deactivate commands.
- Loading an aggregate by `Id` for domain mutation.
- Loading an aggregate by a natural/business key such as `Code` or `StudentNumber` when an insert/update/delete invariant requires the complete entity.
- Transactions, optimistic concurrency and unit-of-work behavior.

### Dapper is required for
- Paged endpoints.
- Search/filter endpoints.
- Dashboard/read-model endpoints.
- Reports.
- Lookup lists that do not require domain behavior.
- Any read where only a subset of columns is needed.

### Projection rule
Paged/list/search queries MUST explicitly name projected properties. BLOB/image/document columns are excluded unless the endpoint explicitly exists to retrieve them.

### No duplicate mapping
`DapperReadStore` obtains table/schema/column identifiers from EF metadata. EF entity configurations therefore remain the single mapping source for both EF writes and Dapper reads.

### By-id policy
Public GET-by-id endpoints may use a dedicated Dapper detail projection where performance warrants it. EF GetById/GetByCode is reserved primarily for command-side/domain workflows that require a tracked or complete aggregate.

### Concurrency
`RowVersion` is an optimistic concurrency token. PostgreSQL does not have SQL Server `rowversion`; SmartSchool stores a `bytea` token and refreshes it on update using a database trigger.
