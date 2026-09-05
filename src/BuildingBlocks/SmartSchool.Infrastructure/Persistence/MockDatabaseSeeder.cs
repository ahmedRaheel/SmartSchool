using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SmartSchool.SharedKernel;

namespace SmartSchool.Infrastructure.Persistence;

/// <summary>
/// Seeds representative records for SmartSchool entities
/// in the development database.
/// </summary>
public sealed class MockDatabaseSeeder(
    ApplicationDbContext dbContext)
{
    /// <summary>
    /// Gets the tenant identifier used by development seed data.
    /// </summary>
    public static readonly Guid DemoTenantId =
        Guid.Parse("11111111-1111-1111-1111-111111111111");

    private const int RecordsPerEntity = 3;

    /// <summary>
    /// Creates development records for supported entity types.
    /// </summary>
    /// <param name="cancellationToken">
    /// Token used to cancel the asynchronous operation.
    /// </param>
    public async Task SeedAsync(
        CancellationToken cancellationToken = default)
    {
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);

        var entityTypes = dbContext.Model
            .GetEntityTypes()
            .Select(entityType => entityType.ClrType)
            .Where(IsSeedableEntity)
            .ToList();

        foreach (var entityType in entityTypes)
        {
            if (await TenantHasDataAsync(
                    entityType,
                    cancellationToken))
            {
                continue;
            }

            SeedEntityType(entityType);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static bool IsSeedableEntity(Type entityType)
    {
        return typeof(Entity).IsAssignableFrom(entityType)
            && !entityType.IsAbstract
            && entityType.GetMethod(
                "Create",
                BindingFlags.Public | BindingFlags.Static) is not null;
    }

    private async Task<bool> TenantHasDataAsync(
        Type entityType,
        CancellationToken cancellationToken)
    {
        var setMethod = typeof(DbContext)
            .GetMethods()
            .Single(method =>
                method.Name == nameof(DbContext.Set)
                && method.IsGenericMethodDefinition
                && method.GetParameters().Length == 0);

        var genericSetMethod = setMethod.MakeGenericMethod(entityType);

        var queryable = genericSetMethod.Invoke(
            dbContext,
            null);

        if (queryable is not IQueryable query)
        {
            return false;
        }

        foreach (var item in query)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (item is Entity entity &&
                entity.TenantId == DemoTenantId)
            {
                return true;
            }
        }

        return false;
    }

    private void SeedEntityType(Type entityType)
    {
        var createMethod = entityType.GetMethod(
            "Create",
            BindingFlags.Public | BindingFlags.Static);

        if (createMethod is null || createMethod.GetParameters().Length != 4)
        {
            return;
        }

        for (var index = 1;
             index <= RecordsPerEntity;
             index++)
        {
            var entity = CreateSeedEntity(
                entityType,
                createMethod,
                index);

            if (entity is null)
            {
                continue;
            }

            dbContext.Add(entity);
        }
    }

    private static object? CreateSeedEntity(
        Type entityType,
        MethodInfo createMethod,
        int index)
    {
        var entityName = entityType.Name.Replace(
            "Entity",
            string.Empty,
            StringComparison.Ordinal);

        var code =
            $"DEMO-{entityName.ToUpperInvariant()}-{index:000}";

        var name =
            $"Demo {entityName} {index}";

        const string metadata =
            "{\"source\":\"ef-inmemory-seed\"}";

        return createMethod.Invoke(
            null,
            [
                DemoTenantId,
                code,
                name,
                metadata
            ]);
    }
}
