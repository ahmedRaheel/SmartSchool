# SmartSchool HybridCache

SmartSchool uses `HybridCache` as the application cache abstraction.

Default topology:

Application -> HybridCache
- L1: local process memory
- L2: PostgreSQL `Infrastructure.DistributedCache`

`Cache:Provider` defaults to `PostgreSql`. `Redis` remains available for
higher-throughput deployments and `Memory` is available for local/test scenarios.

PostgreSQL is therefore the default infrastructure platform for:
- relational school data
- pgvector RAG embeddings
- Hangfire persistence
- distributed L2 cache

Expired cache rows can be removed periodically by a Hangfire maintenance job.
Business handlers should consume HybridCache rather than depend directly on
PostgreSQL or Redis cache implementations.
