using Microsoft.AspNetCore.Http;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Application.Http;

public static class HttpResultExtensions
{
    public static IResult ToHttpResult<T>(this Result<T> result, HttpContext? context = null)
    {
        var traceId = context?.TraceIdentifier ?? string.Empty;
        if (result.IsSuccess)
            return Results.Ok(ApiResponse<T>.Ok(result.Value, traceId));

        return Failure<T>(result.Error, traceId);
    }

    public static IResult ToHttpResult(this Result result, HttpContext? context = null)
    {
        var traceId = context?.TraceIdentifier ?? string.Empty;
        if (result.IsSuccess)
            return Results.Ok(ApiResponse<object?>.Ok(null, traceId));

        return Failure<object?>(result.Error, traceId);
    }

    private static IResult Failure<T>(Error error, string traceId)
    {
        var body = ApiResponse<T>.Fail(error.Code, error.Message, traceId);
        return error.Code switch
        {
            ErrorCodes.Validation => Results.Json(body, statusCode: StatusCodes.Status400BadRequest),
            ErrorCodes.NotFound => Results.Json(body, statusCode: StatusCodes.Status404NotFound),
            ErrorCodes.Conflict => Results.Json(body, statusCode: StatusCodes.Status409Conflict),
            ErrorCodes.Unauthorized => Results.Json(body, statusCode: StatusCodes.Status401Unauthorized),
            _ => Results.Json(body, statusCode: StatusCodes.Status500InternalServerError)
        };
    }
}
