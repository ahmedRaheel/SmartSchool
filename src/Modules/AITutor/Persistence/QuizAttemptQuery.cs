using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// EF-backed read persistence for QuizAttemptEntity.
/// </summary>
public sealed class QuizAttemptQuery(IEfMockStore store) : IQuizAttemptQuery
{
	public Task<QuizAttemptEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<QuizAttemptEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<QuizAttemptEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<QuizAttemptEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<QuizAttemptEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
