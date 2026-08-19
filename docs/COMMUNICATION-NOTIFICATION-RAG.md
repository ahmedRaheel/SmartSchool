# Communication, Notifications and RAG
Chat participants support Student, Parent, Teacher, Principal, Admin, Exam and Academics roles. Persist membership and validate tenant + membership on every message read/write. Use SignalR for real-time client delivery; use Kafka/Outbox for cross-module business events.

Notifications are recipient-specific. Exam, Finance, Leave, Admission, Academic, Attendance, Transport and Events modules publish domain/integration events; the Communication module creates in-app notifications and can fan out to push/email/SMS without coupling the originating transaction.

RAG samples are in `samples/RAG/SchoolKnowledge`. Ingestion should use Ollama embeddings, tenant/document ACL metadata and citation-required retrieval. Never leak CNIC, B-Form, medical, payroll, passwords or tokens into prompts.

## Local Ollama
SmartSchool RAG is configured for local Ollama at `http://localhost:11434`.
The default sample chat model is `llama3.2` and embedding model is `nomic-embed-text`.
Both are configurable through `AI:Ollama`. No hosted OpenAI dependency is required
for the sample RAG pipeline.
