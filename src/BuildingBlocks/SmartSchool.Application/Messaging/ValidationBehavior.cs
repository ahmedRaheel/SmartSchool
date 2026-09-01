using FluentValidation;
using SmartSchool.Application.Messaging;

public sealed class ValidationBehavior<TRequest, TResponse>(
	IEnumerable<IValidator<TRequest>> validators)
	: IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	public async Task<TResponse> HandleAsync(
		TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		var validatorList = validators.ToArray();

		if (validatorList.Length == 0)
		{
			return await next();
		}

		var context = new ValidationContext<TRequest>(request);

		var validationResults = await Task.WhenAll(
			validatorList.Select(validator =>
				validator.ValidateAsync(
					context,
					cancellationToken)));

		var failures = validationResults
			.SelectMany(result => result.Errors)
			.Where(failure => failure is not null)
			.ToArray();

		if (failures.Length > 0)
		{
			throw new ValidationException(
				"One or more validation errors occurred.",
				failures);
		}

		return await next();
	}
}
