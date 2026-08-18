using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Learning.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Learning.Persistence;

/// <summary>
/// EF-backed read persistence for AssignmentEntity.
/// </summary>
public sealed class AssignmentQuery(IEfMockStore store) : IAssignmentQuery
{
	public Task<AssignmentEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<AssignmentEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<AssignmentEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<AssignmentEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<AssignmentEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
