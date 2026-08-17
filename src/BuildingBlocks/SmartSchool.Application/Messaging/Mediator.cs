using Microsoft.Extensions.DependencyInjection;

namespace SmartSchool.Application.Messaging;

public interface IRequest<TResponse>
{
}

public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> HandleAsync(
        TRequest request,
        CancellationToken cancellationToken);
}

public interface IMediator
{
    Task<TResponse> SendAsync<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>;
}

public sealed class Mediator(
    IServiceProvider serviceProvider) : IMediator
{
    public Task<TResponse> SendAsync<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>
    {
        var handler = serviceProvider
            .GetRequiredService<IRequestHandler<TRequest, TResponse>>();

        return handler.HandleAsync(request, cancellationToken);
    }
}

public static class MediatorRegistration
{
    public static IServiceCollection AddSmartSchoolMediator(
        this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator>();
        return services;
    }
}
