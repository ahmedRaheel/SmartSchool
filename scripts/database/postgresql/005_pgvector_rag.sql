-- SmartSchool local/hosted PostgreSQL RAG support.
-- Requires the pgvector extension to be available on the PostgreSQL server.
CREATE EXTENSION IF NOT EXISTS vector;

CREATE SCHEMA IF NOT EXISTS "AICore";

CREATE TABLE IF NOT EXISTS "AICore"."RagKnowledgeDocuments" (
    "Id" uuid PRIMARY KEY,
    "TenantId" uuid NOT NULL,
    "CollectionId" uuid NULL,
    "Title" varchar(300) NOT NULL,
    "SourceName" varchar(500) NOT NULL,
    "Audience" varchar(500) NULL,
    "ContentHash" varchar(128) NOT NULL,
    "IsApproved" boolean NOT NULL DEFAULT false,
    "IndexedAt" timestamptz NULL
);

CREATE TABLE IF NOT EXISTS "AICore"."RagKnowledgeChunks" (
    "Id" uuid PRIMARY KEY,
    "TenantId" uuid NOT NULL,
    "DocumentId" uuid NOT NULL,
    "ChunkIndex" integer NOT NULL,
    "Content" text NOT NULL,
    "CitationLabel" varchar(500) NOT NULL,
    "Embedding" vector(768) NULL,
    CONSTRAINT "FK_RagKnowledgeChunks_Document"
        FOREIGN KEY ("DocumentId")
        REFERENCES "AICore"."RagKnowledgeDocuments"("Id")
        ON DELETE CASCADE,
    CONSTRAINT "UQ_RagKnowledgeChunk"
        UNIQUE ("TenantId", "DocumentId", "ChunkIndex")
);

CREATE INDEX IF NOT EXISTS "IX_RagKnowledgeChunks_TenantDocument"
    ON "AICore"."RagKnowledgeChunks" ("TenantId", "DocumentId");

-- HNSW supports fast approximate cosine similarity search.
CREATE INDEX IF NOT EXISTS "IX_RagKnowledgeChunks_Embedding_Hnsw"
    ON "AICore"."RagKnowledgeChunks"
    USING hnsw ("Embedding" vector_cosine_ops)
    WHERE "Embedding" IS NOT NULL;
