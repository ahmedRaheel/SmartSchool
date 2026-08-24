using System.Diagnostics;
using System.Text.Json;
using Dapper;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Infrastructure.Errors;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService,
    IDbConnectionFactory connectionFactory)
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
            "Unhandled exception processing {Method} {Path}. TraceId={TraceId}, CorrelationId={CorrelationId}",
            httpContext.Request.Method,
            httpContext.Request.Path,
            traceId,
            correlationId);

        await PersistFailureAsync(httpContext, exception, traceId, correlationId, cancellationToken);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = ErrorMessages.UnexpectedError,
            Detail = ErrorMessages.RequestFailed,
            Type = ProblemTypeUris.InternalServerError
        };
        problem.Extensions["traceId"] = traceId;
        problem.Extensions["correlationId"] = correlationId;

        return await problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext { HttpContext = httpContext, ProblemDetails = problem });
    }

    private async Task PersistFailureAsync(
        HttpContext context,
        Exception exception,
        string traceId,
        string correlationId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO observability.application_log
                (level, service, message, message_template, exception, trace_id,
                 correlation_id, request_path, properties)
            VALUES
                ('ERROR', 'SmartSchool.Api', @Message, @MessageTemplate, @Exception, @TraceId,
                 @CorrelationId, @RequestPath, CAST(@Properties AS jsonb));
            """;

        try
        {
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            await connection.ExecuteAsync(new CommandDefinition(
                sql,
                new
                {
                    Message = $"{exception.GetType().Name}: {exception.Message}",
                    MessageTemplate = "Unhandled exception processing {Method} {Path}",
                    Exception = exception.ToString(),
                    TraceId = traceId,
                    CorrelationId = correlationId,
                    RequestPath = context.Request.Path.Value,
                    Properties = JsonSerializer.Serialize(new
                    {
                        method = context.Request.Method,
                        queryString = context.Request.QueryString.Value,
                        exceptionType = exception.GetType().FullName,
                        innerException = exception.InnerException?.Message
                    })
                },
                cancellationToken: cancellationToken));
        }
        catch (Exception persistenceException)
        {
            logger.LogWarning(persistenceException, "Failed to persist exception to observability.application_log");
        }
    }
}
