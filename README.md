# SmartSchool production backend foundation
.NET 10 modular monolith + Vertical Slice style feature folders. 24 modules, 116 domain model scaffolds, Result Pattern, authenticated Minimal APIs, central packages/build configuration, structured Serilog enrichment, global exception handling with Problem Details, Options/IOptionsMonitor-ready configuration, Development/Staging/Production settings, Hangfire PostgreSQL, idempotent Kafka producer foundation, RAG/AI contracts, Python ML prediction client, tenant-aware entity base and audit/concurrency fields.

## Important
This is a broad production-grade foundation, not a claim that every school-specific business rule is finished. Repository implementations, EF mappings/migrations, exact authorization policies, validation rules, transactional Outbox, SignalR chat, payment gateways, model-provider adapters and individual workflows must be implemented/tested against the final database and business rules before production.

## Design
Use Kafka for cross-boundary integration events (enrollment, exam result published, payment, notification, analytics/prediction refresh), not synchronous CRUD. Use Hangfire for durable jobs (prediction refresh, payroll, fee reminders, report cards, certificate batches, RAG ingestion, outbox recovery). Keep LLM/RAG/agents in .NET; use Python for ML training/inference. Never let AI tools bypass normal tenant/role/guardian/student authorization.
