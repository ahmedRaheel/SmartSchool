using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Examinations.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Examinations.Persistence;

/// <summary>
/// EF-backed read persistence for ExamEntity.
/// </summary>
public sealed class ExamQuery(IEfMockStore store) : IExamQuery
{
	public Task<ExamEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<ExamEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<ExamEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<ExamEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<ExamEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
