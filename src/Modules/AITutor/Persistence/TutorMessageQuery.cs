using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// EF-backed read persistence for TutorMessageEntity.
/// </summary>
public sealed class TutorMessageQuery(IEfMockStore store) : ITutorMessageQuery
{
	public Task<TutorMessageEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<TutorMessageEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<TutorMessageEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<TutorMessageEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<TutorMessageEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
