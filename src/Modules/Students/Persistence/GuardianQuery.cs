using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Persistence;

/// <summary>
/// EF-backed read persistence for GuardianEntity.
/// </summary>
public sealed class GuardianQuery(IEfMockStore store) : IGuardianQuery
{
	public Task<GuardianEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<GuardianEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<GuardianEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<GuardianEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<GuardianEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
