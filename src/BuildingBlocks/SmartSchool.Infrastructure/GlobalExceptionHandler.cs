using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartSchool.Infrastructure.Options;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Infrastructure.Errors;

/// <summary>
/// Converts unhandled exceptions to ProblemDetails and logs the full failure through
/// the configured Microsoft ILogger/Serilog pipeline. Persistence is a logging concern;
/// this handler deliberately has no database dependency.
/// </summary>
public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService,
    IOptions<ErrorHandlingOptions> errorHandlingOptions)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.TraceId.ToString() ?? httpContext.TraceIdentifier;
        var correlationId = httpContext.Request.Headers[ApiRoutes.CorrelationHeader].FirstOrDefault()
            ?? httpContext.TraceIdentifier;

        logger.LogError(
            exception,
            "Unhandled exception processing {RequestMethod} {RequestPath}. TraceId={TraceId}, CorrelationId={CorrelationId}",
            httpContext.Request.Method,
            httpContext.Request.Path,
            traceId,
            correlationId);

        httpContext.Response.Headers[ApiRoutes.CorrelationHeader] = correlationId;
        httpContext.Response.Headers[ApiRoutes.TraceHeader] = traceId;
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = ErrorMessages.UnexpectedError,
            Detail = ErrorMessages.RequestFailed,
            Type = errorHandlingOptions.Value.InternalServerErrorTypeUri
        };

        problem.Extensions["traceId"] = traceId;
        problem.Extensions["correlationId"] = correlationId;

        return await problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problem
            });
    }
}
