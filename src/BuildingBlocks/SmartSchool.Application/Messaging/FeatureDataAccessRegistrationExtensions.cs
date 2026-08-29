using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace SmartSchool.Application.Messaging;

/// <summary>
/// Registers feature-local query and command implementations by convention.
/// A concrete type ending in Query or Command is registered against the
/// feature-local interface it implements. This keeps module registration
/// independent from individual entities and allows slices to evolve without
/// editing Module.cs for every data-access type.
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
                && type.Namespace is not null
                && type.Namespace.Contains(".Features.", StringComparison.Ordinal))
            .ToArray();

        foreach (var implementation in implementations)
        {
            var contracts = implementation
                .ImplementedInterfaces
                .Where(contract =>
                    contract.DeclaringType is not null
                    && contract.DeclaringType.Namespace is not null
                    && contract.DeclaringType.Namespace.Contains(".Features.", StringComparison.Ordinal));

            foreach (var contract in contracts)
            {
                services.AddScoped(contract, implementation.AsType());
            }
        }

        return services;
    }
}
