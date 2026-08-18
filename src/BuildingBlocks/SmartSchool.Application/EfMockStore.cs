using SmartSchool.SharedKernel;

namespace SmartSchool.Application.Persistence;

/// <summary>
/// Provides EF-backed development persistence while preserving module-specific query and command contracts.
/// </summary>
public interface IEfMockStore
{
	Task<TEntity?> GetByIdAsync<TEntity>(Guid tenantId, Guid id, CancellationToken cancellationToken) where TEntity : Entity;
	Task<PagedResult<TEntity>> GetPageAsync<TEntity>(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken) where TEntity : Entity;
	Task<bool> ExistsByCodeAsync<TEntity>(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken) where TEntity : Entity;
	Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : Entity;
	Task UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : Entity;
	Task DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : Entity;
}
