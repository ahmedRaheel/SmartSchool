# SmartSchool Coding Standards

## Readability first
Production code must not be compressed into one-line declarations or one-line methods.
A developer should be able to debug a handler without reformatting the file first.

## SOLID
- **S**: Each vertical slice handles one use case.
- **O**: Infrastructure is hidden behind abstractions.
- **L**: Implementations honor their contracts.
- **I**: Complex aggregates should use purpose-specific repository interfaces.
- **D**: Handlers depend on abstractions, not EF Core or Kafka directly.

## DRY

Do not create generic repositories and do not repeat repository contracts inside features.
Each model has explicit query and command abstractions such as `IStudentQuery` and
`IStudentCommand`. Shared cross-cutting behavior belongs in infrastructure or decorators.


## KISS
Handlers explicitly show validation, lookup, mutation, persistence, and result mapping.
Avoid unnecessary mediator/framework layers unless they provide measurable value.

## Vertical Slice
Commands/queries, validators, handlers and endpoint mapping are grouped by use case.
Shared infrastructure does not contain business rules.

## Validation
FluentValidation validators are explicit classes and registered in DI.
Expected validation/business failures return `Result<T>`. Unexpected failures are handled
centrally and returned as Problem Details.

## Tenant security
The tenant identifier shown in scaffold endpoints is a development contract only.
Production handlers must resolve the tenant from authenticated context and must never trust
a caller-supplied tenant id without authorization.

## Persistence
The shared generic repository is appropriate only for simple CRUD. Enrollment, exam
publication, payroll, fee payment, messaging and AI workflows should use aggregate-specific
repositories and transactions.

## Integration
Use transactional Outbox before publishing Kafka integration events. Kafka is not a CRUD bus.
Use Hangfire for durable/background work, not ordinary request/response execution.
