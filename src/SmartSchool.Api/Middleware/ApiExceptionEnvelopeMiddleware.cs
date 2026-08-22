using SmartSchool.SharedKernel;

namespace SmartSchool.Api.Middleware;

public sealed class ApiExceptionEnvelopeMiddleware(RequestDelegate next, ILogger<ApiExceptionEnvelopeMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try { await next(context); }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled API error. TraceId {TraceId}", context.TraceIdentifier);
            if (context.Response.HasStarted) throw;
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(
                ApiResponse<object?>.Fail("internal_error", "An unexpected error occurred.", context.TraceIdentifier));
        }
    }
}
