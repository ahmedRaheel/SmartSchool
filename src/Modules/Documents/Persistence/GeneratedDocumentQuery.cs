using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Documents.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Persistence;

/// <summary>
/// EF-backed read persistence for GeneratedDocumentEntity.
/// </summary>
public sealed class GeneratedDocumentQuery(IEfMockStore store) : IGeneratedDocumentQuery
{
	public Task<GeneratedDocumentEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<GeneratedDocumentEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<GeneratedDocumentEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<GeneratedDocumentEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<GeneratedDocumentEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
