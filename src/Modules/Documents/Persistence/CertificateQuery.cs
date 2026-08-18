using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Documents.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Persistence;

/// <summary>
/// EF-backed read persistence for CertificateEntity.
/// </summary>
public sealed class CertificateQuery(IEfMockStore store) : ICertificateQuery
{
	public Task<CertificateEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<CertificateEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<CertificateEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<CertificateEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<CertificateEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
