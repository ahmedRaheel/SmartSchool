using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Infrastructure.Persistence;

/// <summary>
/// Implements CRUD operations and paging against the EF Core development database.
/// </summary>
public sealed class EfMockStore(ApplicationDbContext dbContext) : IEfMockStore
{
    public Task<TEntity?> GetByIdAsync<TEntity>(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
        where TEntity : Entity
    {
        return dbContext
            .Set<TEntity>()
            .AsNoTracking()
            .SingleOrDefaultAsync(
                entity => entity.TenantId == tenantId && EF.Property<Guid>(entity, GetPrimaryKeyName<TEntity>()) == id,
                cancellationToken);
    }

    public async Task<PagedResult<TEntity>> GetPageAsync<TEntity>(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
        where TEntity : Entity
    {
        var safePage = Math.Max(page, 1);
        var safePageSize = Math.Clamp(pageSize, 1, 200);

        var query = dbContext
            .Set<TEntity>()
            .AsNoTracking()
            .Where(entity => entity.TenantId == tenantId);

        var totalCount = await query.LongCountAsync(cancellationToken);

        var items = await query
            .OrderBy(entity => entity.CreatedAt)
            .ThenBy(entity => EF.Property<Guid>(entity, GetPrimaryKeyName<TEntity>()))
            .Skip((safePage - 1) * safePageSize)
            .Take(safePageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<TEntity>(items, safePage, safePageSize, totalCount);
    }

    public async Task AddAsync<TEntity>(
        TEntity entity,
        CancellationToken cancellationToken)
        where TEntity : Entity
    {
        await dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync<TEntity>(
        TEntity entity,
        CancellationToken cancellationToken)
        where TEntity : Entity
    {
        dbContext.Set<TEntity>().Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync<TEntity>(
        TEntity entity,
        CancellationToken cancellationToken)
        where TEntity : Entity
    {
        dbContext.Set<TEntity>().Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    private string GetPrimaryKeyName<TEntity>() where TEntity : Entity
    {
        var entityType = dbContext.Model.FindEntityType(typeof(TEntity))
            ?? throw new InvalidOperationException($"{typeof(TEntity).Name} is not mapped by EF Core.");

        var primaryKey = entityType.FindPrimaryKey()
            ?? throw new InvalidOperationException($"{typeof(TEntity).Name} does not define a primary key.");

        if (primaryKey.Properties.Count != 1)
        {
            throw new InvalidOperationException($"{typeof(TEntity).Name} must have a single entity-specific primary key.");
        }

        return primaryKey.Properties[0].Name;
    }


}
