# SmartSchool C# Code Standards

- C# indentation uses tabs with a visual width of four spaces.
- Types, namespaces, methods and public properties use PascalCase.
- Parameters and local variables use descriptive camelCase names.
- Avoid abbreviations such as `ct`, `ctx`, `cfg`, `req`, `res`, `cmd` and `qry`. Prefer `cancellationToken`, `dbContext`, `configuration`, `request`, `response`, `command` and `query`.
- Cancellation parameters are named `cancellationToken` consistently.
- Do not compress methods, conditionals, loops or persistence operations onto one line.
- Use braces for method bodies and multiline control flow.
- Keep one responsibility per method and use blank lines to separate logical steps.
- Prefer intention-revealing names such as `safePageSize`, `totalCount`, `normalizedCode`, `codeProperty` and `entityBuilder`.
- Public domain members continue to require XML documentation.
- Module-specific command/query abstractions remain in place; no generic repository is introduced into feature handlers.
