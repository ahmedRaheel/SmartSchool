using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// EF-backed read persistence for CourseOfferingEntity.
/// </summary>
public sealed class CourseOfferingQuery(IEfMockStore store) : ICourseOfferingQuery
{
	public Task<CourseOfferingEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<CourseOfferingEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<CourseOfferingEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<CourseOfferingEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<CourseOfferingEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
