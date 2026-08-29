# Feature-Owned Vertical Slice Data Access

Global rule:
- Everything specific to a use case lives in that feature file: request/query/command, response, validator, handler, feature-specific interface, and implementation.
- Read features use Dapper and project SQL directly into the feature Response/read model. They do not materialize EF/domain entities merely to map them to a response.
- Create, Update, and Delete use EF Core and entities/aggregates. Update/Delete may load the entity because mutation/domain behavior requires it.
- Shared infrastructure only remains outside slices: DbContext, IDbConnectionFactory, EF configurations, interceptors, migrations, transactions/outbox and genuinely shared services.
- Feature data-access registrations are convention-based at module assembly level; Module.cs must not register every entity query/command manually.
- Do not extract feature-specific persistence into Persistence merely to satisfy a folder boundary. Extract only when a capability becomes genuinely shared or infrastructure-level.
