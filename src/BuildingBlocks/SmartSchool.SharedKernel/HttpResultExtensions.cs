using Microsoft.AspNetCore.Http;

namespace SmartSchool.SharedKernel;

public static class HttpResultExtensions
{
    public static IResult ToHttpResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return result.Error.Code switch
        {
            "validation" => Results.BadRequest(result.Error),
            "not_found" => Results.NotFound(result.Error),
            "conflict" => Results.Conflict(result.Error),
            "unauthorized" => Results.Unauthorized(),
            _ => Results.Problem(
                title: "Request failed",
                detail: result.Error.Message)
        };
    }
}
