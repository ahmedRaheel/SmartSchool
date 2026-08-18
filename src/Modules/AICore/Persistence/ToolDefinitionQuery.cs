using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// EF-backed read persistence for ToolDefinitionEntity.
/// </summary>
public sealed class ToolDefinitionQuery(IEfMockStore store) : IToolDefinitionQuery
{
	public Task<ToolDefinitionEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<ToolDefinitionEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<ToolDefinitionEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<ToolDefinitionEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<ToolDefinitionEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
