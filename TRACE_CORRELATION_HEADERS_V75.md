# v75 - Correlation and Trace IDs in API response headers

All API responses now expose request identifiers in response headers:

- `X-Correlation-ID`: preserves a caller-supplied correlation ID or generates one when absent.
- `X-Trace-Id`: uses the active distributed-tracing `Activity.TraceId`, falling back to `HttpContext.TraceIdentifier`.

The global exception handler also writes both headers explicitly before producing ProblemDetails, while retaining `traceId` and `correlationId` in the error body. Serilog LogContext uses the same identifiers so a client-reported ID can be searched directly in logs/database telemetry.
