using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Examinations.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Examinations.Persistence;

/// <summary>
/// EF-backed read persistence for ExamSubjectEntity.
/// </summary>
public sealed class ExamSubjectQuery(IEfMockStore store) : IExamSubjectQuery
{
	public Task<ExamSubjectEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<ExamSubjectEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<ExamSubjectEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<ExamSubjectEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<ExamSubjectEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
