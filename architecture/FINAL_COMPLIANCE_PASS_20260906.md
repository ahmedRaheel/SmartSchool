# SmartSchool Backend — Remaining Architecture Compliance Pass

## Scope
All backend code except `src/Modules/Identity` and `src/SmartSchool.Identity.Api`. Those two Identity projects were treated as immutable and verified by SHA-256 before/after this pass.

## Changes completed

1. **Persistence boundary completed**
   - Moved every non-Identity module `*DbContext.cs` out of `Persistence` into `Infrastructure/Data` while preserving namespaces/contracts to avoid unnecessary reference churn.
   - Non-Identity module `Persistence` folders now contain only EF Core entity configurations.

2. **Vertical Slice/CQRS gate completed**
   - Zero non-Identity shared `I*Query` / `I*Command` interfaces.
   - Added/retained executable architecture gates to prevent reintroduction.

3. **Async / HTTP / testability standards**
   - Removed remaining `EnsureSuccessStatusCode()` use outside excluded Identity projects and preserved provider response/status in exceptions.
   - No direct `new HttpClient`, `async void`, `.Result/.Wait`, or `throw ex` violations in audited backend scope.
   - Replaced direct `DateTime.Now/UtcNow` usage with injected `TimeProvider` in affected runtime paths and registered `TimeProvider.System` once at composition root.

4. **Academic hierarchy contract**
   - `ClassSectionEntity.GradeLevelId` is required, matching the domain rule `Campus -> GradeLevel -> ClassSection`.
   - EF mapping explicitly requires `grade_level_id`.
   - Existing-database migration enforces required campus/grade relationships for new/updated rows with `NOT VALID` check constraints, allowing legacy rows to be remediated safely before validation.
   - Canonical consolidated schema now creates `grade_level` and `class_section` with the current fields instead of depending on repair ALTERs for fresh installations.

5. **Finance schema contract**
   - Canonical `finance.fee_type` now creates `frequency`, `description`, and `metadata_json` from the start and uses the same code/name lengths as EF.
   - This removes the fresh-install source of the `metadata_json` mismatch previously seen at runtime.

## Automated static compliance gate
Run:

```bash
./architecture/verify-backend-architecture.sh
```

The gate checks non-Identity code for:
- shared `I*Query/I*Command` abstractions;
- non-configuration source under module Persistence folders;
- sync-over-async;
- `async void`;
- direct `new HttpClient`;
- `EnsureSuccessStatusCode` hiding provider failures;
- direct `DateTime.Now/UtcNow` instead of `TimeProvider`;
- `throw ex` stack-loss.

All gates pass in this package.

## Identity integrity
`architecture/identity-project-integrity-final.sha256` records the excluded Identity source. The before/after manifests matched exactly during this pass.

## Verification boundary
Static architecture compliance is complete for the rules above. The execution image does not provide the .NET SDK, so compile/runtime certification (`dotnet restore`, `dotnet build`, automated tests, clean PostgreSQL deployment, and API smoke tests) cannot truthfully be claimed from this environment. Those are execution checks, not unaddressed static architecture exceptions.
