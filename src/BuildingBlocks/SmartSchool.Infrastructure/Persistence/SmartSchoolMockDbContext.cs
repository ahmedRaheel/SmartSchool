using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SmartSchool.SharedKernel;

namespace SmartSchool.Infrastructure.Persistence;

/// <summary>
/// EF Core in-memory database used for development and end-to-end mock API execution.
/// </summary>
public sealed class SmartSchoolMockDbContext(
    DbContextOptions<SmartSchoolMockDbContext> options)
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entityTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(assembly =>
                !assembly.IsDynamic
                && assembly.GetName().Name?.StartsWith(
                    "SmartSchool.Modules.",
                    StringComparison.Ordinal) == true)
            .SelectMany(GetLoadableTypes)
            .Where(type =>
                !type.IsAbstract
                && typeof(Entity).IsAssignableFrom(type));

        foreach (var entityType in entityTypes)
        {
            var entityBuilder = modelBuilder.Entity(entityType);

            entityBuilder.Property(nameof(Entity.TenantId)).IsRequired();
            entityBuilder.Property(nameof(Entity.IsActive)).IsRequired();
            entityBuilder.Property(nameof(Entity.RowVersion)).IsConcurrencyToken();

            if (entityType.GetProperty("Code") is not null)
            {
                entityBuilder
                    .HasIndex(nameof(Entity.TenantId), "Code")
                    .IsUnique();
            }
        }
    }

    private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException exception)
        {
            return exception.Types.OfType<Type>();
        }
    }
}
