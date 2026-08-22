-- PostgreSQL L2 distributed cache used by ASP.NET Core HybridCache.
CREATE SCHEMA IF NOT EXISTS "Infrastructure";

CREATE TABLE IF NOT EXISTS "Infrastructure"."DistributedCache" (
    "Id" text NOT NULL PRIMARY KEY,
    "Value" bytea NOT NULL,
    "ExpiresAtTime" timestamptz NOT NULL,
    "SlidingExpirationInSeconds" bigint NULL,
    "AbsoluteExpiration" timestamptz NULL
);

CREATE INDEX IF NOT EXISTS "IX_DistributedCache_ExpiresAtTime"
    ON "Infrastructure"."DistributedCache" ("ExpiresAtTime");
