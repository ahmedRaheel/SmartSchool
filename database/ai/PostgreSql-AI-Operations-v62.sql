CREATE EXTENSION IF NOT EXISTS vector;
CREATE SCHEMA IF NOT EXISTS ai_core;
CREATE SCHEMA IF NOT EXISTS ai_inquiry;
CREATE SCHEMA IF NOT EXISTS ai_tutor;

CREATE TABLE IF NOT EXISTS ai_core.rag_knowledge_chunk(
 id uuid PRIMARY KEY,
 tenant_id uuid NOT NULL,
 collection varchar(100) NOT NULL,
 document_name varchar(300) NOT NULL,
 content text NOT NULL,
 embedding vector(768) NOT NULL,
 created_at timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP,
 is_active boolean NOT NULL DEFAULT true
);
CREATE INDEX IF NOT EXISTS ix_rag_chunk_tenant_collection ON ai_core.rag_knowledge_chunk(tenant_id,collection,is_active);
-- Create after sufficient rows exist:
-- CREATE INDEX ix_rag_chunk_embedding_hnsw ON ai_core.rag_knowledge_chunk USING hnsw (embedding vector_cosine_ops);

-- The rich operational payload for existing generated CRUD entities is stored in MetadataJson.
-- Existing EF migrations own ai_core/ai_inquiry/ai_tutor entity tables.
