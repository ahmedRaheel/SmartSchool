using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace SmartSchool.Application.Messaging;

/// <summary>
/// Registers feature-owned query and command services by convention.
/// CQRS request/message types are deliberately excluded from self-registration.
/// </summary>
public static class FeaturePersistenceRegistrationExtensions
{
    public static IServiceCollection AddFeaturePersistence(
        this IServiceCollection services,
        Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(assembly);

        var implementations = assembly
            .DefinedTypes
            .Where(type =>
                type is { IsAbstract: false, IsInterface: false }
                && IsFeatureType(type))
            .ToArray();

        foreach (var implementation in implementations)
        {
            var implementationType = implementation.AsType();

            // Feature contracts are the authoritative registration mechanism.
            // This supports both top-level and nested feature-owned interfaces.
            foreach (var contract in implementation.ImplementedInterfaces.Where(IsFeatureContract))
            {
                services.AddScoped(contract, implementationType);
            }

            // A small number of feature-local services are consumed by their
            // concrete type. Only top-level Query/Command implementations may
            // be self-registered. Nested Command/Query records are mediator
            // messages containing runtime values (Guid, string, etc.) and must
            // never be created by the DI container.
            if (!implementation.IsNested && IsQueryOrCommandImplementation(implementation))
            {
                services.AddScoped(implementationType);
            }
        }

        return services;
    }

    private static bool IsFeatureType(TypeInfo type)
    {
        return type.Namespace?.Contains(".Features", StringComparison.Ordinal) == true;
    }

    private static bool IsFeatureContract(Type contract)
    {
        return contract.IsInterface
            && contract.Namespace?.Contains(".Features", StringComparison.Ordinal) == true
            && (contract.Name.EndsWith("Query", StringComparison.Ordinal)
                || contract.Name.EndsWith("Command", StringComparison.Ordinal));
    }

    private static bool IsQueryOrCommandImplementation(TypeInfo type)
    {
        return type.Name.EndsWith("Query", StringComparison.Ordinal)
            || type.Name.EndsWith("Command", StringComparison.Ordinal);
    }
}
