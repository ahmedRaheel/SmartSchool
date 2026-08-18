using System.Threading.Tasks;
using SmartSchool.SharedKernel;

namespace SmartSchool.Application.Persistence;

/// <summary>
/// Generic persistence contract for simple aggregate CRUD.
/// Complex aggregates should expose purpose-specific repositories instead.
/// </summary>
public interface IRepository<TEntity>
	where TEntity : Entity
{
	Task<TEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken);

	Task<PagedResult<TEntity>> GetPageAsync(
		Guid tenantId,
		int page,
		int pageSize,
		CancellationToken cancellationToken);

	Task AddAsync(
		TEntity entity,
		CancellationToken cancellationToken);

	void Remove(TEntity entity);

	Task<bool> ExistsByCodeAsync(
		Guid tenantId,
		string code,
		Guid? excludingId,
		CancellationToken cancellationToken);

	Task SaveChangesAsync(CancellationToken cancellationToken);
}
