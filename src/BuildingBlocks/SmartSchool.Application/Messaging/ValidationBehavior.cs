using FluentValidation;
using SmartSchool.SharedKernel;

namespace SmartSchool.Application.Messaging;

/// <summary>Executes feature validators before the request handler.</summary>
public sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <inheritdoc />
    public async Task<TResponse> HandleAsync(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var registeredValidators = validators.ToArray();

        if (registeredValidators.Length == 0)
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
        var results = await Task.WhenAll(
            registeredValidators.Select(
                validator => validator.ValidateAsync(
                    context,
                    cancellationToken)));

        var messages = results
            .SelectMany(result => result.Errors)
            .Where(error => error is not null)
            .Select(error => error.ErrorMessage)
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        if (messages.Length == 0)
        {
            return await next();
        }

        return ResultFactory.CreateValidationFailure<TResponse>(
            string.Join("; ", messages));
    }
}
