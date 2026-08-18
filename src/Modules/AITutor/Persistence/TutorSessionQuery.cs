using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// EF-backed read persistence for TutorSessionEntity.
/// </summary>
public sealed class TutorSessionQuery(IEfMockStore store) : ITutorSessionQuery
{
	public Task<TutorSessionEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<TutorSessionEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<TutorSessionEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<TutorSessionEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<TutorSessionEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
