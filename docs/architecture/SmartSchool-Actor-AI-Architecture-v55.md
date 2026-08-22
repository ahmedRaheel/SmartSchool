# SmartSchool Actor + AI Architecture v55

## Authorization boundary
SuperAdmin = platform scope. SchoolAdmin = tenant/school scope. Teacher = assigned class/subject scope. Student = self scope. Parent = linked-child scope. Driver = assigned route/vehicle scope. Examiner = assigned examination scope.

RBAC alone is not sufficient. Every data query must enforce TenantId plus actor relationship (teacher assignment, guardian link, route assignment, exam assignment).

## Runtime
Portal -> IdentityServer -> SmartSchool.Api.
PostgreSQL is system of record; pgvector stores tenant-scoped RAG embeddings.
Redis is distributed cache for feature flags, sessions/read models where configured.
Kafka is asynchronous integration/event backbone (notifications, ingestion, prediction/workflow triggers).
Ollama provides local chat + embeddings.
OpenTelemetry/Serilog provide trace/correlation/logging.

## AI
Documents/notes -> ingestion job -> chunk -> Ollama embeddings -> pgvector.
Question -> actor/tenant authorization -> embedding -> top-k pgvector -> Ollama grounded answer -> citations.
Predictions remain in AIPrediction (ML.NET/external prediction client) and should be refreshed by jobs/events, not calculated in dashboard controllers.

## Important
Feature flags hide/disable tenant capabilities but are not authorization. API policies remain authoritative.
Impersonation must use server-issued short-lived tokens and preserve impersonator_sub; never fake the role in React.
