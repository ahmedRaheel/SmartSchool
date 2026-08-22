# Observability v53

- Fixes the Identity response corruption by registering `X-Trace-Id` and `X-Correlation-ID` with `Response.OnStarting`.
- OpenTelemetry traces/metrics remain enabled.
- Serilog writes structured logs to console and rolling daily files under `logs/` (30 days).
- Portal browser errors are posted to `/api/telemetry/ui-errors`; the server logs them through Serilog and returns trace/correlation IDs.
- PostgreSQL and SQL Server `observability.application_log` DDL is included under `database/observability`.

## Database logging
The table is included for durable searchable logs. File + console logging are enabled by default. Database log persistence should be enabled with a provider-specific Serilog sink only after choosing the active provider; do not synchronously write every request log to the application database in production. OpenTelemetry + centralized log storage is preferred for high-volume telemetry.

## Security
Never log passwords, access tokens, refresh tokens, authorization headers, or client secrets.
