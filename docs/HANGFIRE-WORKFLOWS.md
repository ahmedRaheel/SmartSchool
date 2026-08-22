# Hangfire workflow architecture

`SmartSchool.BackgroundJobs` is the orchestration project for durable background work.

Jobs included:
- exam start/reminders
- fee due/overdue reminders
- Eid/summer/winter/public holiday notifications
- timetable/class timing notifications
- leave workflows
- admission workflows
- event workflows
- result publication
- attendance notifications
- local Ollama RAG knowledge ingestion
- notification delivery

Business rules remain inside their owning modules. Hangfire jobs orchestrate feature commands and must be idempotent.

Use immediate Hangfire jobs for deferred work, scheduled jobs for a known future time, recurring jobs for periodic scans, and continuation jobs for multi-step workflows. Cross-module business events should still use Outbox/Kafka.

Storage switches with `Database:Provider`: PostgreSQL uses Hangfire.PostgreSql and SQL Server uses Hangfire.SqlServer.
