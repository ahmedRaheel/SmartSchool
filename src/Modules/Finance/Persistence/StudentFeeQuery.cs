using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Finance.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// EF-backed read persistence for StudentFeeEntity.
/// </summary>
public sealed class StudentFeeQuery(IEfMockStore store) : IStudentFeeQuery
{
	public Task<StudentFeeEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<StudentFeeEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<StudentFeeEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<StudentFeeEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<StudentFeeEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
