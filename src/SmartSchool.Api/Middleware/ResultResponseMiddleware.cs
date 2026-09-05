using System.Text.Json;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Api.Middleware;

/// <summary>
/// Transitional safety net for legacy /api endpoints that have not yet been refactored to return Result.
/// New endpoints must return Result/Result&lt;T&gt; directly.
/// </summary>
public sealed class ResultResponseMiddleware(RequestDelegate next, ILogger<ResultResponseMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments("/api"))
        {
            await next(context);
            return;
        }

        var originalBody = context.Response.Body;
        await using var buffer = new MemoryStream();
        context.Response.Body = buffer;

        try
        {
            await next(context);

            buffer.Position = 0;
            var content = await new StreamReader(buffer).ReadToEndAsync();
            context.Response.Body = originalBody;

            if (IsAlreadyResult(content))
            {
                await context.Response.WriteAsync(content);
                return;
            }

            var response = context.Response.StatusCode is >= 200 and < 300 ? CreateSuccess(content) : CreateFailure(context.Response.StatusCode, content);

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception exception)
        {
            context.Response.Body = originalBody;
            logger.LogError(exception, "Unhandled API exception. TraceId {TraceId}", context.TraceIdentifier);

            if (context.Response.HasStarted)
            {
                throw;
            }

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(
                Result<object?>.Failure(Error.InternalServerError("An unexpected error occurred.")));
        }
    }

    private static bool IsAlreadyResult(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return false;
        }

        try
        {
            using var document = JsonDocument.Parse(content);
            return document.RootElement.ValueKind == JsonValueKind.Object
                && document.RootElement.TryGetProperty("isSuccess", out _)
                && document.RootElement.TryGetProperty("error", out _);
        }
        catch (JsonException)
        {
            return false;
        }
    }

    private static object CreateSuccess(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return Result<object?>.Success(null);
        }

        try
        {
            var value = JsonSerializer.Deserialize<JsonElement>(content);
            return Result<JsonElement>.Success(value);
        }
        catch (JsonException)
        {
            return Result<string>.Success(content);
        }
    }

    private static object CreateFailure(int statusCode, string content)
    {
        var message = ExtractMessage(content) ?? statusCode switch
        {
            StatusCodes.Status400BadRequest => "The request is invalid.",
            StatusCodes.Status401Unauthorized => "Authentication is required or the access token is invalid.",
            StatusCodes.Status403Forbidden => "You do not have permission to perform this operation.",
            StatusCodes.Status404NotFound => "The requested resource was not found.",
            StatusCodes.Status409Conflict => "The request conflicts with the current state.",
            _ => "The request could not be completed."
        };

        var error = statusCode switch
        {
            StatusCodes.Status400BadRequest => Error.Validation(message),
            StatusCodes.Status401Unauthorized => Error.Unauthorized(message),
            StatusCodes.Status403Forbidden => Error.Forbidden(message),
            StatusCodes.Status404NotFound => Error.NotFound(message),
            StatusCodes.Status409Conflict => Error.Conflict(message),
            _ => Error.InternalServerError(message)
        };

        return Result<object?>.Failure(error);
    }

    private static string? ExtractMessage(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return null;
        }

        try
        {
            using var document = JsonDocument.Parse(content);
            var root = document.RootElement;

            foreach (var property in new[] { "detail", "message", "title" })
            {
                if (root.ValueKind == JsonValueKind.Object
                    && root.TryGetProperty(property, out var value)
                    && value.ValueKind == JsonValueKind.String)
                {
                    return value.GetString();
                }
            }
        }
        catch (JsonException)
        {
            // Non-JSON framework response. Use the standard status message.
        }

        return null;
    }
}
