using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Admissions.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Admissions.Persistence;

/// <summary>
/// EF-backed read persistence for AdmissionDecisionEntity.
/// </summary>
public sealed class AdmissionDecisionQuery(IEfMockStore store) : IAdmissionDecisionQuery
{
	public Task<AdmissionDecisionEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<AdmissionDecisionEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<AdmissionDecisionEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<AdmissionDecisionEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<AdmissionDecisionEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
