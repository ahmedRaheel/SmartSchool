using SmartSchool.SharedKernel;

namespace SmartSchool.Api.Middleware;

/// <summary>
/// Legacy compatibility middleware. ResultResponseMiddleware is the canonical API response boundary.
/// </summary>
public sealed class ApiExceptionEnvelopeMiddleware(
	RequestDelegate next,
	ILogger<ApiExceptionEnvelopeMiddleware> logger)
{
	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await next(context);
		}
		catch (Exception exception)
		{
			logger.LogError(exception, "Unhandled API error. TraceId {TraceId}", context.TraceIdentifier);

			if (context.Response.HasStarted)
			{
				throw;
			}

			context.Response.Clear();
			context.Response.StatusCode = StatusCodes.Status500InternalServerError;
			await context.Response.WriteAsJsonAsync(
				Result<object?>.Failure(
					Error.InternalServerError("An unexpected error occurred.")));
		}
	}
}
