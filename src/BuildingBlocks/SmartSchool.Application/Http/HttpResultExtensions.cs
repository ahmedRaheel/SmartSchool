using Microsoft.AspNetCore.Http;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Application.Http;

/// <summary>
/// Converts application results to ASP.NET Core HTTP results.
/// </summary>
public static class HttpResultExtensions
{
    /// <summary>Converts a typed application result to an HTTP result.</summary>
    public static IResult ToHttpResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return ToFailureResult(result.Error);
    }

    /// <summary>Converts an application result to an HTTP result.</summary>
    public static IResult ToHttpResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return Results.NoContent();
        }

        return ToFailureResult(result.Error);
    }

    private static IResult ToFailureResult(Error error) =>
        error.Code switch
        {
            ErrorCodes.Validation => Results.BadRequest(error),
            ErrorCodes.NotFound => Results.NotFound(error),
            ErrorCodes.Conflict => Results.Conflict(error),
            ErrorCodes.Unauthorized => Results.Unauthorized(),
            _ => Results.Problem(
                title: ErrorMessages.RequestFailed,
                detail: error.MessageEntity,
                statusCode: StatusCodes.Status500InternalServerError)
        };
}
