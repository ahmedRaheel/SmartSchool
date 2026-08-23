# SmartSchool Backend v61

Implemented runtime paths:
- JWT policy fix: `Admin` is now treated as a tenant administrator alongside `SchoolAdmin`; SuperAdmin remains platform scoped.
- Tenant scope service: normal actors take tenant from the JWT and cannot override it; SuperAdmin is not automatically tenant-filtered.
- Campus and notification list endpoints use authenticated tenant scope; SuperAdmin list reads can be cross-tenant.
- Dashboard admin aggregation is cross-tenant for SuperAdmin and token-tenant scoped for other administrators.
- SignalR ChatHub persists messages, verifies conversation membership/tenant, publishes Kafka event, then broadcasts. Notification creation persists, publishes Kafka, then pushes SignalR.
- Kafka producer is exposed behind `IIntegrationEventPublisher`; API hosts a real Kafka notification-request consumer.
- RAG has a real pgvector table, Ollama embedding/generation calls, Redis response caching, tenant isolation and Kafka audit/event publishing.
- Chatbots: student, teacher, parent, admissions, admin with role and collection-specific retrieval policies.
- RAG document indexing endpoint creates embeddings and writes pgvector rows.
- EF configuration audit: every entity configuration now supplies a schema to `ToTable`; migration helper included for mappings that previously used public.

Run before RAG:
`database/ai/PostgreSql-Rag-v61.sql`

Review/run for pre-existing public tables before deploying schema mapping changes:
`database/schema/PostgreSql-v61-module-schemas.sql`
