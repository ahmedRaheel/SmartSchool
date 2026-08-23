# SmartSchool v66 — CAG-first AI and impersonation

## AI execution
The backend now uses CAG first and pgvector RAG as fallback. `AiContextService` is owned by AICore and uses `HybridCache`; with the existing Redis provider this gives local L1 + Redis L2 context caching. Cache keys include tenant, school, actor, assistant, collections and a knowledge version. Indexing knowledge rotates the tenant knowledge version, so stale contexts stop being used without wildcard Redis deletes.

`POST /api/chatbots/{bot}/ask` uses the shared AICore context service. It no longer caches only final answers. The cached object is authorized context and citations. If cached context does not appear to cover the question, pgvector retrieval is used.

## Impersonation
`POST /api/identity/users/impersonation/start` is allowed by the Impersonation policy (SuperAdmin, SchoolAdmin, Admin). Tenant admins are restricted to their own tenant and cannot impersonate SuperAdmin.

A Duende extension grant named `impersonation` now performs the actual token exchange. It requires a valid administrator `actor_token` and `target_user_id`; the issued subject is the target user and the token carries `impersonated=true` and `impersonator_sub=<original subject>`.

Token request fields:
- grant_type=impersonation
- client_id=smartschool-login-api
- client_secret=<configured secret>
- actor_token=<administrator access token>
- target_user_id=<target user guid>
- reason=<support reason>
- scope=openid profile email smartschool.profile smartschool.api

The login client seeder now allows the impersonation extension grant. Re-run the Identity configuration seeder (or update the persisted Duende client grant type) when upgrading an existing database.
