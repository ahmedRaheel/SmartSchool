using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// EF-backed read persistence for ModelConfigurationEntity.
/// </summary>
public sealed class ModelConfigurationQuery(IEfMockStore store) : IModelConfigurationQuery
{
	public Task<ModelConfigurationEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<ModelConfigurationEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<ModelConfigurationEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<ModelConfigurationEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<ModelConfigurationEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
