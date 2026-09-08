using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace SmartSchool.Application.Messaging;

/// <summary>
/// Registers feature-owned query and command services by convention.
/// Both top-level and nested feature contracts are supported, and concrete
/// query/command classes are self-registered for feature-local composition.
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

            // Some slices compose feature-local concrete Query/Command services
            // directly (for example validation queries used by a command).
            if (IsQueryOrCommand(implementation))
            {
                services.AddScoped(implementationType);
            }

            var contracts = implementation
                .ImplementedInterfaces
                .Where(IsFeatureContract)
                .ToArray();

            foreach (var contract in contracts)
            {
                services.AddScoped(contract, implementationType);
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
        return contract.Namespace?.Contains(".Features", StringComparison.Ordinal) == true;
    }

    private static bool IsQueryOrCommand(TypeInfo type)
    {
        return type.Name.EndsWith("Query", StringComparison.Ordinal)
            || type.Name.EndsWith("Command", StringComparison.Ordinal);
    }
}
