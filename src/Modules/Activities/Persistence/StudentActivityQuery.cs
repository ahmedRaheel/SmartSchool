using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Activities.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Activities.Persistence;

/// <summary>
/// EF-backed read persistence for StudentActivityEntity.
/// </summary>
public sealed class StudentActivityQuery(IEfMockStore store) : IStudentActivityQuery
{
	public Task<StudentActivityEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<StudentActivityEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<StudentActivityEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<StudentActivityEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<StudentActivityEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
