# SmartSchool backend architecture compliance pass

This pass enforces the agreed direction: strict feature ownership, Persistence reserved for EF mapping/infrastructure, Dapper reads, EF writes, tenant isolation, typed configuration, explicit authorization, and actionable provider failures.

## Corrected in this pass
- Removed all Query/Command slice files from module `Persistence` folders (moved without namespace churn to avoid unrelated behavioral changes).
- Reworked `OllamaClient` to use typed `OllamaRagOptions` via `IOptionsMonitor` instead of direct `IConfiguration` reads.
- Added startup validation for Ollama BaseUrl, chat model, and embedding model.
- Added batch embedding to the single Ollama client and removed duplicate PDF-upload embedding HTTP/configuration logic.
- Replaced hidden Ollama `EnsureSuccessStatusCode()` failure in the assistant path with an exception containing operation, model, HTTP status, and provider response.
- Configured the named Ollama HttpClient from typed options.
- Added `/api/ai/model-config` compatibility route with optional page/pageSize defaults so the existing UI URL resolves.
- AI knowledge contribution policy explicitly includes Tenant/Owner plus Principal, Teacher, Examiner, HR Manager, Accountant/Finance, and SuperAdmin.
- Added an executable static architecture gate at `architecture/verify-compliance.sh`.

## Verification boundary
The supplied execution environment has Node.js but no .NET SDK (`dotnet` is not installed). Therefore a truthful 100% compile/runtime certification cannot be issued here. Run `dotnet restore`, `dotnet build`, and the test suite in the target .NET SDK/CI environment. Static architecture gates included in this package pass in this environment.
