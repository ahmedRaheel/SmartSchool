# Compliance status

This package is an in-progress hardening build and is intentionally not labelled 100% compliant.

Completed in this pass:
- Consolidated redundant AI physical schemas to ai_core, ai, ai_tutor.
- Converted 108 generated EF read-query implementations to Dapper-only persistence.
- Preserved EF Core command/write boundary.
- Existing architecture guard remains authoritative.

Before a 100% compliance claim, every remaining item emitted by `python build/architecture/verify_architecture.py` must be zero and the solution must compile/test successfully.
