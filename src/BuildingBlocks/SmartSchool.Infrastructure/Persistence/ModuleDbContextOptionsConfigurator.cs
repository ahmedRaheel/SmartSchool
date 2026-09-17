using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pgvector.EntityFrameworkCore;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Infrastructure.Persistence;

/// <summary>
/// Applies the host-selected database provider to module-owned DbContexts.
/// Connection strings are resolved through IConfiguration only.
/// </summary>
public sealed class ModuleDbContextOptionsConfigurator(
    AuditSaveChangesInterceptor auditSaveChangesInterceptor)
    : IModuleDbContextOptionsConfigurator
{
    public void Configure<TContext>(
        IConfiguration configuration,
        DbContextOptionsBuilder options,
        string migrationsHistorySchema)
        where TContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(options);

        var persistence = configuration
            .GetSection(PersistenceOptions.SectionName)
            .Get<PersistenceOptions>() ?? new PersistenceOptions();

        options.AddInterceptors(auditSaveChangesInterceptor);

        switch (persistence.Provider)
        {
            case PersistenceProvider.Mock:
                options.UseInMemoryDatabase($"SmartSchool-{typeof(TContext).Name}");
                break;

            case PersistenceProvider.PostgreSql:
                ConfigurePostgreSql<TContext>(
                    configuration,
                    options,
                    persistence,
                    migrationsHistorySchema);
                break;

            case PersistenceProvider.SqlServer:
                ConfigureSqlServer<TContext>(
                    configuration,
                    options,
                    persistence,
                    migrationsHistorySchema);
                break;

            default:
                throw new InvalidOperationException(
                    $"Unsupported persistence provider '{persistence.Provider}'.");
        }

        if (persistence.EnableSensitiveDataLogging)
        {
            options.EnableSensitiveDataLogging();
        }
    }

    private static void ConfigurePostgreSql<TContext>(
        IConfiguration configuration,
        DbContextOptionsBuilder options,
        PersistenceOptions persistence,
        string migrationsHistorySchema)
        where TContext : DbContext
    {
        var connectionString = GetRequiredConnectionString(
            configuration,
            persistence.ConnectionStringName);

        options.UseNpgsql(
            connectionString,
            providerOptions =>
            {
                providerOptions.EnableRetryOnFailure(5);
                providerOptions.UseVector();
                providerOptions.MigrationsAssembly(typeof(TContext).Assembly.FullName);
                providerOptions.MigrationsHistoryTable(
                    GetHistoryTableName<TContext>(),
                    migrationsHistorySchema);
            });
    }

    private static void ConfigureSqlServer<TContext>(
        IConfiguration configuration,
        DbContextOptionsBuilder options,
        PersistenceOptions persistence,
        string migrationsHistorySchema)
        where TContext : DbContext
    {
        var connectionString = GetRequiredConnectionString(
            configuration,
            persistence.ConnectionStringName);

        options.UseSqlServer(
            connectionString,
            providerOptions =>
            {
                providerOptions.EnableRetryOnFailure(5);
                providerOptions.MigrationsAssembly(typeof(TContext).Assembly.FullName);
                providerOptions.MigrationsHistoryTable(
                    GetHistoryTableName<TContext>(),
                    migrationsHistorySchema);
            });
    }

    private static string GetRequiredConnectionString(
        IConfiguration configuration,
        string connectionStringName)
    {
        var connectionString = configuration.GetConnectionString(connectionStringName);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"Connection string '{connectionStringName}' is required " +
                "for the configured persistence provider.");
        }

        return connectionString;
    }

    private static string GetHistoryTableName<TContext>()
        where TContext : DbContext
    {
        const string prefix = "__EFMigrationsHistory_";
        const int postgreSqlIdentifierLimit = 63;

        var contextName = typeof(TContext).Name;
        var availableLength = postgreSqlIdentifierLimit  - prefix.Length;

        if (contextName.Length > availableLength)
        {
            contextName = contextName[..availableLength];
        }

        return prefix + contextName;
    }
}
