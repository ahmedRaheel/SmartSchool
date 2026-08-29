# SmartSchool Architecture Review v12

## Enforced architecture

- Vertical Slice: endpoint/request/response/validator/handler per use case.
- Handlers orchestrate only. No SQL, `IDbConnectionFactory`, or `DbContext` in feature handlers.
- Query persistence uses Dapper only.
- Command persistence uses EF Core only.
- `IHttpClientFactory`/typed or named clients only; no `new HttpClient()`.
- `System.Text.Json` only; Newtonsoft.Json is prohibited.
- `ConfigureAwait(false)` is appropriate in reusable infrastructure/library code where context capture is unnecessary. It is not mechanically added to ASP.NET Core request code because ASP.NET Core has no request SynchronizationContext and blanket use adds noise without benefit.
- `ValueTask` is used only where synchronous completion is common or an API contract benefits from it. It is not a replacement for every `Task`.
- No application-owned tables in PostgreSQL `public` schema.

## Database review

The legacy `public.driverdirectoryread`, `public.studentdirectoryread`, `public.teacherdirectoryread`, and `public.schooldocument` tables were generated read models and are not referenced by application code. Their EF models/configurations were removed and migration `V120__remove_public_legacy_read_models.sql` drops them. Canonical document ownership remains in the document aggregate.

Materialized views should be introduced only for expensive, stable read projections with measurable benefit. They are not a blanket replacement for Dapper SQL: parameterized point reads, tenant-filtered lists, transactional/current data, and pgvector similarity queries should remain normal Dapper queries. A materialized view introduces refresh/staleness semantics and is justified only after query-plan/latency evidence.

## Automated architecture gate

Run:

```bash
python build/architecture/verify_architecture.py
```

The gate fails when it finds direct SQL/DbContext/connection access in features, EF Core in Query classes, Dapper in Command classes, direct `HttpClient` construction, or Newtonsoft.Json.

## Review status of uploaded solution

The uploaded solution is not yet globally compliant with the requested persistence boundary. The audit found a large amount of older generated persistence code where Query classes still use EF Core and several feature endpoints still access persistence directly. The gate intentionally reports these rather than hiding them. These must be migrated module-by-module without changing runtime behavior.

The v12 changes remove public-schema legacy entities, add the architecture gate, and correct the Subject creation slice so its Dapper branch lookup lives in persistence rather than the handler. No claim is made that all pre-existing modules are already compliant.
