using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartSchool.Infrastructure.Persistence;

namespace SmartSchool.Infrastructure;

public static class PersistenceRegistration
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("SmartSchool");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "ConnectionStrings:DefaultConnection is not configured.");
        }

        services.AddDbContext<SmartSchoolDbContext>(
            options =>
            {
                options.UseNpgsql(
                    connectionString,
                    postgreSql =>
                    {
                        postgreSql.MigrationsAssembly(
                            typeof(SmartSchoolDbContext)
                                .Assembly
                                .FullName);
                    });
            });

        return services;
    }
}
