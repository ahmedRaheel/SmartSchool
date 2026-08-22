# v55 Architect polish

Implemented in this package:
- Seven-actor role/policy model (SuperAdmin, SchoolAdmin, Teacher, Student, Parent, Driver, Examiner).
- Role-aware portal navigation.
- Platform tenant feature flags backed by IDistributedCache (Redis when configured).
- Actor-policy dashboard protection.
- Functional Ollama + pgvector RAG ask endpoint and portal AI Assistant.
- Tenant-scoped retrieval and citations.
- Docker Ollama model configuration.
- pgvector schema upgrade script.
- Existing Communication, Notification, Workflow, AIPrediction and Kafka/Redis infrastructure retained.
- Existing observability/login fixes retained.

Production-hardening still required:
- Relationship-level authorization handlers for teacher-class, parent-child, driver-route and examiner-exam must be applied consistently to every legacy CRUD endpoint.
- Impersonation token exchange/custom grant remains to be completed; v54 intentionally did not forge tokens.
- Chat drawer still contains legacy local seed state; the Communication module APIs are the authoritative backend and the full drawer should be migrated to them.
- Prediction quality requires real historical school data/training/evaluation; software cannot honestly guarantee prediction accuracy without it.
- Run the pgvector v55 SQL migration before using the new RAG endpoint.
