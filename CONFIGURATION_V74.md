# SmartSchool v74 configuration cleanup

- Serilog file, console and PostgreSQL sinks are configured in code from strongly typed `LoggingOptions`.
- PostgreSQL sink writes to the configured schema/table and uses the configured connection-string name.
- `GlobalExceptionHandler` has no database dependency; it only emits structured logs.
- Authentication authority, metadata address, issuer, audience and portal URL are configuration-driven.
- Identity API CORS origins are configuration-driven.
- Ollama, ML and Identity token endpoint URL fallbacks were removed from C#; missing required configuration now fails explicitly instead of silently using localhost/Docker URLs.
- ProblemDetails type URI moved to `ErrorHandling` configuration.
- Hard-coded HTTP/HTTPS URLs were removed from runtime C# source. Environment-specific URLs remain in appsettings/launch/Docker configuration, where they belong.
