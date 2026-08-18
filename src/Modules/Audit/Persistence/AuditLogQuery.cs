using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Audit.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Audit.Persistence;

/// <summary>
/// EF-backed read persistence for AuditLogEntity.
/// </summary>
public sealed class AuditLogQuery(IEfMockStore store) : IAuditLogQuery
{
	public Task<AuditLogEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<AuditLogEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<AuditLogEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<AuditLogEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<AuditLogEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
