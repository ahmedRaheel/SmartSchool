using Microsoft.AspNetCore.Http;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.SharedKernel;

public static class HttpResultExtensions
{
    public static Result ToHttpResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Result.Ok(result.Value);
        }

        return result.Error.Code switch
        {
            ErrorCodes.Validation => Results.BadRequest(result.Error),
            ErrorCodes.NotFound => Results.NotFound(result.Error),
            ErrorCodes.Conflict => Results.Conflict(result.Error),
            ErrorCodes.Unauthorized => Results.Unauthorized(),
            _ => Results.Problem(
                title: ErrorMessages.RequestFailed,
                detail: result.Error.Message)
        };
    }
}
