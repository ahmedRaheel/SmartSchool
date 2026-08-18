using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// EF-backed read persistence for StudentInterventionEntity.
/// </summary>
public sealed class StudentInterventionQuery(IEfMockStore store) : IStudentInterventionQuery
{
	public Task<StudentInterventionEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<StudentInterventionEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<StudentInterventionEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<StudentInterventionEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<StudentInterventionEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
