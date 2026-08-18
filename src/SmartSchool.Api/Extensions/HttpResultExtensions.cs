using Microsoft.AspNetCore.Http;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;


namespace SmartSchool.Application.Http;

public static class HttpResultExtensions
{
	public static IResult ToHttpResult<T>(
		this Result<T> result)
	{
		if (result.IsSuccess)
		{
			return Results.Ok(result.Value);
		}

		return ToProblemResult(result.Error);
	}

	public static IResult ToHttpResult(
		this Result result)
	{
		if (result.IsSuccess)
		{
			return Results.NoContent();
		}

		return ToProblemResult(result.Error);
	}

	private static IResult ToProblemResult(
		Error error)
	{
		return error.Code switch
		{
			ErrorCodes.Validation =>
				Results.BadRequest(error),

			ErrorCodes.NotFound =>
				Results.NotFound(error),

			ErrorCodes.Conflict =>
				Results.Conflict(error),

			ErrorCodes.Unauthorized =>
				Results.Unauthorized(),

			_ =>
				Results.Problem(
					title: ErrorMessages.RequestFailed,
					detail: error.Message,
					statusCode:
						StatusCodes.Status500InternalServerError)
		};
	}
}
