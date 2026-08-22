CREATE EXTENSION IF NOT EXISTS vector;
ALTER TABLE ai.knowledge_chunk ADD COLUMN IF NOT EXISTS content text NULL;
ALTER TABLE ai.knowledge_chunk ADD COLUMN IF NOT EXISTS embedding vector(768) NULL;
CREATE INDEX IF NOT EXISTS ix_knowledge_chunk_tenant_active ON ai.knowledge_chunk(tenant_id,is_active);
-- Create HNSW after embeddings exist:
-- CREATE INDEX ix_knowledge_chunk_embedding_hnsw ON ai.knowledge_chunk USING hnsw (embedding vector_cosine_ops);
