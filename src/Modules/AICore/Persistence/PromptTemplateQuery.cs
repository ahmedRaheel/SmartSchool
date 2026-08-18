using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// EF-backed read persistence for PromptTemplateEntity.
/// </summary>
public sealed class PromptTemplateQuery(IEfMockStore store) : IPromptTemplateQuery
{
	public Task<PromptTemplateEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<PromptTemplateEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<PromptTemplateEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<PromptTemplateEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<PromptTemplateEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
