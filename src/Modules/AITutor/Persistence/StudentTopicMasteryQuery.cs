using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// EF-backed read persistence for StudentTopicMasteryEntity.
/// </summary>
public sealed class StudentTopicMasteryQuery(IEfMockStore store) : IStudentTopicMasteryQuery
{
	public Task<StudentTopicMasteryEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<StudentTopicMasteryEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<StudentTopicMasteryEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<StudentTopicMasteryEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<StudentTopicMasteryEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
