# Shared Query/Command Refactor Re-audit — 2026-09-06

## Scope
Non-Identity backend only. SmartSchool.Identity.Api and Modules/Identity were excluded from this change set.

## Fixed
- Removed the remaining shared `I*Query` / `I*Command` persistence interfaces in non-Identity modules.
- Removed duplicate `Features/DataAccess/...` copies for AdmissionWorkflow, EmployeeOnboarding, EmployeeEvidence, and StudentOnboarding.
- Replaced the remaining interface registrations with concrete feature collaborators and removed obsolete interface files.
- Renamed legacy shared persistence implementation types away from `*Query` / `*Command` to explicit `*Reader` / `*Writer` names so CQRS message names (`Query` / `Command`) remain reserved for feature messages.
- Updated all non-Identity consumers and DI registrations consistently.

## Architecture rule
CQRS `Query` and `Command` records inside a feature are messages and are valid. Generic/shared `I*Query` / `I*Command` persistence contracts are prohibited. New use cases should continue the existing feature-owned pattern (`IGet...`, `ICreate...`, `IUpdate...`, etc.) with the implementation owned by that slice.

## Validation performed
Static scans confirm zero non-Identity `interface I*Query` / `interface I*Command` declarations and zero references to the removed interface names. Identity source is excluded from the refactor.

## Build limitation
The execution image used for this refactor does not contain the .NET SDK, so compile/test certification must be run in the repository's normal .NET build environment.
