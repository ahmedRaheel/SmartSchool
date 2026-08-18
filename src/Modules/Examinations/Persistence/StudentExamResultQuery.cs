using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Examinations.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Examinations.Persistence;

/// <summary>
/// EF-backed read persistence for StudentExamResultEntity.
/// </summary>
public sealed class StudentExamResultQuery(IEfMockStore store) : IStudentExamResultQuery
{
	public Task<StudentExamResultEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<StudentExamResultEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<StudentExamResultEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<StudentExamResultEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<StudentExamResultEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
