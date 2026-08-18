using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// EF-backed read persistence for CourseSelectionEntity.
/// </summary>
public sealed class CourseSelectionQuery(IEfMockStore store) : ICourseSelectionQuery
{
	public Task<CourseSelectionEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<CourseSelectionEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<CourseSelectionEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<CourseSelectionEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<CourseSelectionEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
