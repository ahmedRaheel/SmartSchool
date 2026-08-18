using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Admissions.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Admissions.Persistence;

/// <summary>
/// EF-backed read persistence for ApplicantEntity.
/// </summary>
public sealed class ApplicantQuery(IEfMockStore store) : IApplicantQuery
{
	public Task<ApplicantEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<ApplicantEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<ApplicantEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<ApplicantEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<ApplicantEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
