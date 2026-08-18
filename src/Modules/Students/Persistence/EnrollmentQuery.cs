using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Persistence;

/// <summary>
/// EF-backed read persistence for EnrollmentEntity.
/// </summary>
public sealed class EnrollmentQuery(IEfMockStore store) : IEnrollmentQuery
{
	public Task<EnrollmentEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<EnrollmentEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<EnrollmentEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<EnrollmentEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<EnrollmentEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
