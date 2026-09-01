using Microsoft.Extensions.DependencyInjection;

namespace SmartSchool.Application.Messaging;

/// <summary>
/// Marks an application request that produces a response.
/// </summary>
/// <typeparam name="TResponse">The response type.</typeparam>
public interface IRequest<TResponse>
{
}

/// <summary>
/// Handles an application request.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public interface IRequestHandler<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	/// <summary>
	/// Handles the request.
	/// </summary>
	Task<TResponse> HandleAsync(
		TRequest request,
		CancellationToken cancellationToken);
}

/// <summary>
/// Represents the next component in the request pipeline.
/// </summary>
/// <typeparam name="TResponse">The response type.</typeparam>
public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();

/// <summary>
/// Defines cross-cutting behavior around a request handler.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public interface IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	/// <summary>
	/// Processes the request and invokes the next pipeline component.
	/// </summary>
	Task<TResponse> HandleAsync(
		TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken);
}

/// <summary>
/// Dispatches application requests.
/// </summary>
public interface IMediator
{
	/// <summary>
	/// Sends a request through the pipeline to its handler.
	/// </summary>
	Task<TResponse> SendAsync<TRequest, TResponse>(
		TRequest request,
		CancellationToken cancellationToken = default)
		where TRequest : IRequest<TResponse>;
}

/// <summary>
/// Dispatches requests through registered pipeline behaviors
/// before invoking the request handler.
/// </summary>
public sealed class Mediator(IServiceProvider serviceProvider) : IMediator
{
	/// <inheritdoc />
	public Task<TResponse> SendAsync<TRequest, TResponse>(
		TRequest request,
		CancellationToken cancellationToken = default)
		where TRequest : IRequest<TResponse>
	{
		ArgumentNullException.ThrowIfNull(request);

		var handler = serviceProvider.GetRequiredService<
			IRequestHandler<TRequest, TResponse>>();

		var behaviors = serviceProvider
			.GetServices<IPipelineBehavior<TRequest, TResponse>>()
			.ToArray();

		RequestHandlerDelegate<TResponse> pipeline =
			() => handler.HandleAsync(
				request,
				cancellationToken);

		for (var index = behaviors.Length - 1; index >= 0; index--)
		{
			var behavior = behaviors[index];
			var next = pipeline;

			pipeline = () => behavior.HandleAsync(
				request,
				next,
				cancellationToken);
		}

		return pipeline();
	}
}
