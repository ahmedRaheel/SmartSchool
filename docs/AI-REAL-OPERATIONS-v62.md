# AI operations v62

The generated CRUD endpoints remain for administration/audit, but they now accept MetadataJson so they can persist rich domain payloads.

Real operations added:
AICore
- POST /api/aicore/knowledge/index: Ollama embedding -> pgvector + KnowledgeChunk audit row + Kafka ingestion event.
- POST /api/aicore/execute: pgvector retrieval -> grounded Ollama generation -> AiExecutionLog + Kafka event.
- GET /api/aicore/health: Ollama connectivity.

AIInquiry
- POST /api/aiinquiry/operations/conversations
- POST /api/aiinquiry/operations/messages: persists visitor + assistant messages and calls Ollama.
- POST /api/aiinquiry/operations/leads: persists lead + Kafka event.
- POST /api/aiinquiry/operations/handoff: persists handoff + Kafka event.

AITutor
- POST /api/aitutor/operations/sessions: persists tutor session + conversation.
- POST /api/aitutor/operations/ask: persists student/AI messages and calls Ollama.
- POST /api/aitutor/operations/quizzes/generate: Ollama generates structured quiz and persists GeneratedQuiz.
- POST /api/aitutor/operations/recommendations/generate: Ollama recommendation persisted.

Architecture note:
The generic CRUD records are administrative persistence endpoints. Business AI behavior belongs in operational endpoints so POST /knowledge-chunk does not unexpectedly call an LLM, and GET/PUT/DELETE remain deterministic.

RAG:
Use /api/aicore/knowledge/index for actual embedding/indexing. /api/aicore/execute and the existing /api/chatbots/{bot}/ask perform retrieval/generation. Run database/ai/PostgreSql-AI-Operations-v62.sql first.
