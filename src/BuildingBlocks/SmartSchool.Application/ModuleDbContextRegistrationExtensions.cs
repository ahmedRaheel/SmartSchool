using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Application;

/// <summary>
/// Registers a DbContext as an explicit persistence boundary owned by a business module.
/// </summary>
public static class ModuleDbContextRegistrationExtensions
{
    public static IServiceCollection AddModuleDbContext<TContext, TContextContract>(
        this IServiceCollection services,
        IConfiguration configuration,
        string migrationsHistorySchema)
        where TContext : DbContext
        where TContextContract : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        if (!typeof(TContextContract).IsAssignableFrom(typeof(TContext)))
        {
            throw new InvalidOperationException(
                $"{typeof(TContext).Name} must implement {typeof(TContextContract).Name}.");
        }

        if (string.IsNullOrWhiteSpace(migrationsHistorySchema))
        {
            throw new ArgumentException(
                "A migrations history schema is required.",
                nameof(migrationsHistorySchema));
        }

        services.AddDbContext<TContext>((serviceProvider, options) =>
        {
            var configurator = serviceProvider
                .GetRequiredService<IModuleDbContextOptionsConfigurator>();

            configurator.Configure<TContext>(
                configuration,
                options,
                migrationsHistorySchema);
        });

        services.AddScoped<TContextContract>(serviceProvider =>
            (TContextContract)(object)serviceProvider.GetRequiredService<TContext>());

        return services;
    }
}
