using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// EF-backed read persistence for LeaveRequestEntity.
/// </summary>
public sealed class LeaveRequestQuery(IEfMockStore store) : ILeaveRequestQuery
{
	public Task<LeaveRequestEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<LeaveRequestEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<LeaveRequestEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<LeaveRequestEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<LeaveRequestEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
