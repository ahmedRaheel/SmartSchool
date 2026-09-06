# SmartSchool backend architecture re-audit — 2026-09-06

## Scope
All backend projects were re-audited. Per explicit instruction, `src/SmartSchool.Identity.Api` and `src/Modules/Identity` were excluded from modification. Their SHA-256 integrity manifest is in `architecture/identity-project-integrity.sha256`.

## Changes applied in this pass
- Removed 335 obsolete shared Query/Command source files that were no longer consumed by a use-case and removed their obsolete DI registrations.
- Kept use-case-owned `Request/Query`, `Response`, `Validator`, `Handler`, and nested feature persistence contracts intact.
- Removed duplicate Ollama embedding implementation from `AiContextService`; it now uses the single `IOllamaClient` runtime.
- Removed direct Ollama `IConfiguration` access from AICore operational endpoints and AITutor operational endpoints.
- Reused the AICore Ollama client from AITutor instead of duplicating provider HTTP code.
- Corrected operational RAG SQL to use `ai_core.rag_knowledge_chunk.id` consistently.
- Replaced hidden `EnsureSuccessStatusCode()` behavior in AIPrediction with an error containing status and provider response.
- Preserved the Identity projects byte-for-byte relative to the input package.

## Remaining architectural exceptions requiring use-case decomposition
A smaller set of workflow-oriented shared Query/Command contracts remains where multiple operations currently depend on one aggregate persistence abstraction (HR employee onboarding, Student admission/onboarding, Organization campus/school policy, Admissions workflow, notification, prediction, exam result, and a few operational AI/Tutor persistence contracts). These are explicitly detectable and must not be confused with full compliance.

This package therefore improves compliance substantially but is not falsely certified as 100% until those workflow contracts are decomposed and the solution is compiled/tested with the target .NET SDK.
