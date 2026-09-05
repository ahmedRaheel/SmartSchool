using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SmartSchool.Application.Messaging;

namespace SmartSchool.Application;

/// <summary>
/// Registers SmartSchool mediator infrastructure.
/// </summary>
public static class ApplicationRegistration
{
    /// <summary>
    /// Registers the mediator, validation pipeline, handlers, and validators
    /// discovered in the supplied feature assemblies.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assemblies">Assemblies containing SmartSchool features.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddSmartSchoolMediator(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Every module calls AddSmartSchoolMediator for its own feature assembly.
        // Infrastructure registrations therefore MUST be idempotent; otherwise each
        // module adds another ValidationBehavior and a single request traverses the
        // same validation pipeline dozens of times.
        services.TryAddScoped<IMediator, Mediator>();
        services.TryAddEnumerable(
            ServiceDescriptor.Scoped(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>)));

        RegisterImplementations(
            services,
            assemblies,
            typeof(IRequestHandler<,>));

        RegisterImplementations(
            services,
            assemblies,
            typeof(IValidator<>));

        return services;
    }

    private static void RegisterImplementations(
        IServiceCollection services,
        IEnumerable<Assembly> assemblies,
        Type openGenericServiceType)
    {
        foreach (var implementationType in assemblies
                     .Distinct()
                     .SelectMany(GetLoadableTypes)
                     .Where(type => !type.IsAbstract && !type.IsInterface))
        {
            var serviceTypes = implementationType
                .GetInterfaces()
                .Where(type =>
                    type.IsGenericType
                    && type.GetGenericTypeDefinition() == openGenericServiceType);

            foreach (var serviceType in serviceTypes)
            {
                services.AddScoped(
                    serviceType,
                    implementationType);
            }
        }
    }

    private static IEnumerable<Type> GetLoadableTypes(
        Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException exception)
        {
            return exception.Types
                .Where(type => type is not null)
                .Cast<Type>();
        }
    }
}
