using SmartSchool.SharedKernel;

namespace SmartSchool.Application.Persistence;

/// <summary>
/// Executes optimized read-only projections with Dapper while EF Core remains
/// the write-side ORM and the internal aggregate loader.
/// </summary>
public interface IDapperReadStore
{
	Task<PagedResult<TEntity>> GetPageAsync<TEntity>(
		Guid tenantId,
		int page,
		int pageSize,
		IReadOnlyCollection<string> projectedProperties,
		CancellationToken cancellationToken)
		where TEntity : Entity;

	Task<PagedResult<TEntity>> GetFilteredPageAsync<TEntity>(
		Guid tenantId,
		int page,
		int pageSize,
		IReadOnlyCollection<string> projectedProperties,
		IReadOnlyDictionary<string, object?> filters,
		string orderByProperty,
		bool descending,
		CancellationToken cancellationToken)
		where TEntity : Entity;

	Task<int> CountAsync<TEntity>(
		Guid tenantId,
		IReadOnlyDictionary<string, object?> filters,
		CancellationToken cancellationToken)
		where TEntity : Entity;

	Task<bool> ExistsAsync<TEntity>(
		Guid tenantId,
		string propertyName,
		object value,
		Guid? excludingId,
		CancellationToken cancellationToken)
		where TEntity : Entity;
}
