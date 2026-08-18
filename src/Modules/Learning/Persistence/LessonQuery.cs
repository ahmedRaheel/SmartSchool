using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Learning.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Learning.Persistence;

/// <summary>
/// EF-backed read persistence for LessonEntity.
/// </summary>
public sealed class LessonQuery(IEfMockStore store) : ILessonQuery
{
	public Task<LessonEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<LessonEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<LessonEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<LessonEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<LessonEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
