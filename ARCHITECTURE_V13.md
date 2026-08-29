# SmartSchool Architecture v13

Canonical AI schemas are now limited to:

- `ai_core`: shared AI platform, RAG/CAG, prompts, tools, execution logs, parent/admission assistant conversations.
- `ai`: prediction/ML read and result models.
- `ai_tutor`: student tutoring, mastery, generated quizzes and tutor sessions.

`ai_parent`, `ai_inquiry`, and `ai_prediction` are redundant physical schemas and are consolidated by V121.

Backend invariant: Vertical Slice handlers orchestrate only; reads are Dapper Query classes; writes are EF Core Command classes. No direct SQL/DbContext/connection in feature handlers. System.Text.Json only. HttpClient instances are obtained from IHttpClientFactory/typed clients.
