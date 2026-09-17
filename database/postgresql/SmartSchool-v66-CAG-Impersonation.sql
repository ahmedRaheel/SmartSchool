-- SmartSchool v66 CAG + impersonation
-- CAG requires no new table: it uses HybridCache/Redis and ai_core.rag_knowledge_chunk.
-- Impersonation grant is normally synchronized by DuendeConfigurationSeeder.
-- Existing installations should restart SmartSchool.Identity.Api after deploying v66.

CREATE EXTENSION IF NOT EXISTS vector;
CREATE INDEX IF NOT EXISTS ix_rag_knowledge_chunk_tenant_collection
    ON ai_core.rag_knowledge_chunk (tenant_id, collection, is_active);
