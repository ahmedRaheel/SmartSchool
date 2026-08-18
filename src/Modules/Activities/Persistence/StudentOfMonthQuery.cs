using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Activities.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Activities.Persistence;

/// <summary>
/// EF-backed read persistence for StudentOfMonthEntity.
/// </summary>
public sealed class StudentOfMonthQuery(IEfMockStore store) : IStudentOfMonthQuery
{
	public Task<StudentOfMonthEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<StudentOfMonthEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<StudentOfMonthEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<StudentOfMonthEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<StudentOfMonthEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
