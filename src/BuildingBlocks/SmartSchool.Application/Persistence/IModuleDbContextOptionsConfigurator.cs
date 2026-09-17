using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SmartSchool.Application.Persistence;

/// <summary>
/// Configures a module-owned DbContext using the host's persistence settings.
/// The module owns its context and migrations while the host selects the provider.
/// </summary>
public interface IModuleDbContextOptionsConfigurator
{
    void Configure<TContext>(
        IConfiguration configuration,
        DbContextOptionsBuilder options,
        string migrationsHistorySchema)
        where TContext : DbContext;
}
