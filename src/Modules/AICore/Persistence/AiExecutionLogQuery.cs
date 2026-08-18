using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// EF-backed read persistence for AiExecutionLogEntity.
/// </summary>
public sealed class AiExecutionLogQuery(IEfMockStore store) : IAiExecutionLogQuery
{
	public Task<AiExecutionLogEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<AiExecutionLogEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<AiExecutionLogEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<AiExecutionLogEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<AiExecutionLogEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
