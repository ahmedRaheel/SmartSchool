namespace SmartSchool.Identity.Api.Observability;

public sealed record UiErrorRequest(
    string Message,
    string? Stack,
    string? Url,
    string? Component,
    string? TraceId,
    string? CorrelationId,
    IDictionary<string, string?>? Context);

public static class UiErrorEndpoints
{
    public static IEndpointRouteBuilder MapUiErrorEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/telemetry/ui-errors", (
            UiErrorRequest request,
            HttpContext httpContext,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("SmartSchool.Portal");

            using (logger.BeginScope(new Dictionary<string, object?>
            {
                ["ClientTraceId"] = request.TraceId,
                ["ClientCorrelationId"] = request.CorrelationId,
                ["ClientUrl"] = request.Url,
                ["ClientComponent"] = request.Component
            }))
            {
                logger.LogError(
                    "Portal error: {Message}. Stack: {Stack}. Context: {@Context}",
                    request.Message,
                    request.Stack,
                    request.Context);
            }

            return Results.Accepted(value: new
            {
                traceId = System.Diagnostics.Activity.Current?.TraceId.ToString(),
                correlationId = httpContext.TraceIdentifier
            });
        }).AllowAnonymous();

        return endpoints;
    }
}
