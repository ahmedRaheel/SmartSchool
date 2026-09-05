using Microsoft.AspNetCore.Http;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Application.Http;

/// <summary>
/// Maps the application's existing Result model to HTTP without changing its JSON shape.
/// Result and Result&lt;T&gt; are the single SmartSchool API response contracts.
/// </summary>
public static class HttpResultExtensions
{
    public static IResult ToHttpResult<T>(this Result<T> result) =>
        Results.Json(result, statusCode: GetStatusCode(result));

    public static IResult ToHttpResult(this Result result) =>
        Results.Json(result, statusCode: GetStatusCode(result));

    private static int GetStatusCode(Result result)
    {
        if (result.IsSuccess)
        {
            return StatusCodes.Status200OK;
        }

        return result.Error.Code switch
        {
            ErrorCodes.Validation => StatusCodes.Status400BadRequest,
            ErrorCodes.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorCodes.Forbidden => StatusCodes.Status403Forbidden,
            ErrorCodes.NotFound => StatusCodes.Status404NotFound,
            ErrorCodes.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
