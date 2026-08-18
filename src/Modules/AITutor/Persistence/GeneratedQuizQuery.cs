using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// EF-backed read persistence for GeneratedQuizEntity.
/// </summary>
public sealed class GeneratedQuizQuery(IEfMockStore store) : IGeneratedQuizQuery
{
	public Task<GeneratedQuizEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<GeneratedQuizEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<GeneratedQuizEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<GeneratedQuizEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<GeneratedQuizEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
