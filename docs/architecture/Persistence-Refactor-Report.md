# Dapper/EF persistence refactor report

- **116/116 paged query implementations** now use Dapper explicit projections.
- Notification recipient-scoped paging and unread count use Dapper filtered reads.
- EF remains for internal aggregate GetById/GetByCode/invariant loading and all writes.
- EF commands remain responsible for create/update/delete and SaveChanges.
- Dapper derives physical schema/table/column identifiers from EF metadata, preventing a second hand-maintained mapping.
- PostgreSQL base lifecycle/concurrency synchronization script is included.
- `row_version` remains PostgreSQL `bytea`, matching C# `byte[] RowVersion`.
- Public list/page APIs do not select BLOBs unless the response explicitly requests them.

See `Persistence-Read-Write-Policy.md` for the enforced policy.
