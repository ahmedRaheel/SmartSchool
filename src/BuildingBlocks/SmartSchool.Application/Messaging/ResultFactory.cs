using SmartSchool.SharedKernel;

namespace SmartSchool.Application.Messaging;

/// <summary>Creates result-pattern failures required by pipeline behaviors.</summary>
internal static class ResultFactory
{
	/// <summary>Creates a validation failure for a result response type.</summary>
	internal static TResponse CreateValidationFailure<TResponse>(
		string message)
	{
		var error = Error.Validation(message);

		if (typeof(TResponse) == typeof(Result))
		{
			return (TResponse)(object)Result.Failure(error);
		}

		var responseType = typeof(TResponse);

		if (!responseType.IsGenericType
			|| responseType.GetGenericTypeDefinition() != typeof(Result<>))
		{
			throw new InvalidOperationException(
				"Validation behavior requires a Result response.");
		}

		var method = responseType.GetMethod(
			nameof(Result<object>.Failure),
			[typeof(Error)]);

		return (TResponse)method!.Invoke(null, [error])!;
	}
}
