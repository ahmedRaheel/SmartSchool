using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIParent.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIParent.Persistence;

/// <summary>
/// EF-backed read persistence for ParentToolExecutionEntity.
/// </summary>
public sealed class ParentToolExecutionQuery(IEfMockStore store) : IParentToolExecutionQuery
{
	public Task<ParentToolExecutionEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<ParentToolExecutionEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<ParentToolExecutionEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<ParentToolExecutionEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<ParentToolExecutionEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
