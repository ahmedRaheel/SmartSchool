# SmartSchool v20 consolidated feature checklist

## Chat
- Tenant-scoped conversations
- Student / Parent / Teacher / Principal / Admin / Exam / Academics participants
- Participant membership and read position
- Messages, replies, edits and soft delete
- Attachments
- Feature-local DTO/CQRS additions
- Communication EF configurations and PostgreSQL/SQL Server DDL

## Notifications
- Recipient-specific notifications
- Read/unread state
- Notification preferences
- Exam, timetable, class timing, holidays/vacations, fee, leave, admission,
  event, attendance, assignment, transport, result and announcement notification types
- PostgreSQL and SQL Server lookup seed scripts

## RAG
- Local Ollama configuration
- llama3.2 sample chat model
- nomic-embed-text sample embedding model
- Student, Parent, Teacher, Principal/Admin and Exam/Academic sample documents
- Tenant and role-aware retrieval design
- PostgreSQL pgvector extension, vector chunk table and HNSW index
- Citation-required design
- Hangfire RAG ingestion job

## Workflow automation
- Dedicated SmartSchool.BackgroundJobs project
- Hangfire PostgreSQL and SQL Server storage support
- PostgreSQL is the default provider
- Exam notification workflow
- Fee due/overdue workflow
- Holiday/vacation workflow
- Timetable/class timing workflow
- Leave workflow
- Admission workflow
- Event workflow
- Result publication workflow
- Attendance workflow
- Notification dispatch workflow
- RAG ingestion workflow
- Hangfire dashboard and worker configuration

## Database default
PostgreSQL is the default relational provider. SQL Server remains configurable.
PostgreSQL is also the preferred RAG database because pgvector stores embeddings
beside tenant-scoped relational school data.

## Hybrid cache
- HybridCache L1 local memory
- PostgreSQL distributed L2 cache is the default
- Redis remains configurable
- Memory provider remains configurable
- PostgreSQL distributed-cache DDL included
