using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using FluentValidation;
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

        if (exception is ValidationException validationException)
        {
            logger.LogWarning(
                "Validation failed processing {RequestMethod} {RequestPath}. TraceId={TraceId}, CorrelationId={CorrelationId}, Errors={ValidationErrors}",
                httpContext.Request.Method,
                httpContext.Request.Path,
                traceId,
                correlationId,
                validationException.Errors.Select(error => new { error.PropertyName, error.ErrorMessage }));

            httpContext.Response.Headers[ApiRoutes.CorrelationHeader] = correlationId;
            httpContext.Response.Headers[ApiRoutes.TraceHeader] = traceId;
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            var validationProblem = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation failed.",
                Detail = "One or more validation errors occurred.",
                Type = "https://httpstatuses.com/400"
            };

            validationProblem.Extensions["code"] = "VALIDATION_ERROR";
            validationProblem.Extensions["errors"] = validationException.Errors
                .GroupBy(error => error.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(error => error.ErrorMessage).Distinct().ToArray());
            validationProblem.Extensions["traceId"] = traceId;
            validationProblem.Extensions["correlationId"] = correlationId;

            return await problemDetailsService.TryWriteAsync(
                new ProblemDetailsContext
                {
                    HttpContext = httpContext,
                    ProblemDetails = validationProblem
                });
        }

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
