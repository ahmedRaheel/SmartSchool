using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Examinations.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Examinations.Persistence;

/// <summary>
/// EF-backed read persistence for GradeScaleEntity.
/// </summary>
public sealed class GradeScaleQuery(IEfMockStore store) : IGradeScaleQuery
{
	public Task<GradeScaleEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<GradeScaleEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<GradeScaleEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<GradeScaleEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<GradeScaleEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
