# Constants Refactor Report

- Generic repository references: 0
- C# lines over 180 characters: 0
- Central constants added for errors, fallback messages, routes, authentication,
  configuration sections, application identifiers, Kafka topics, Hangfire jobs/queues,
  external service routes and Problem Details URIs.
- Every module now has `ModuleConstants` for its stable name and route segment.
- Feature handlers use `ErrorMessages` instead of repeated entity-not-found/duplicate-code literals.

Some strings intentionally remain literals where they are not magic identifiers, such as
structured logging message templates and XML documentation. Business-configurable values
remain database concerns rather than constants.
