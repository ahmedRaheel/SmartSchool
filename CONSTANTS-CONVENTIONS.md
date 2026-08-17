# Constants Convention

SmartSchool does not use magic strings for stable technical identifiers.

## Code constants
Use constants for:
- error codes
- fallback error messages
- configuration section names
- API route roots and common routes
- authentication scheme/audience
- application/client names
- correlation headers
- Kafka topics
- Hangfire queue/recurring-job names
- external service routes
- Problem Details type URIs
- module names and route segments

## Do not turn business data into constants
Configurable school values such as exam types, certificate types, academic systems,
notification templates, grade scales, fee types and workflow definitions remain database data.

## Human-facing messages
`ErrorMessages` contains only safe fallback API messages. The target architecture should add
`IMessageProvider`, backed by cache + database templates, for tenant-customizable/localized
business messages. Error codes remain stable even when message text changes.

## Enums/value objects
Closed domain concepts that are truly invariant may be represented as enums/value objects.
If a school administrator can configure the value, it belongs in the database instead.
