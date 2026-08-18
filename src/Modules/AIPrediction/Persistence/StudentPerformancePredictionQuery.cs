using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// EF-backed read persistence for StudentPerformancePredictionEntity.
/// </summary>
public sealed class StudentPerformancePredictionQuery(IEfMockStore store) : IStudentPerformancePredictionQuery
{
	public Task<StudentPerformancePredictionEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<StudentPerformancePredictionEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<StudentPerformancePredictionEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<StudentPerformancePredictionEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<StudentPerformancePredictionEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
