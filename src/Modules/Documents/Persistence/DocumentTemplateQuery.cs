using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Documents.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Persistence;

/// <summary>
/// EF-backed read persistence for DocumentTemplateEntity.
/// </summary>
public sealed class DocumentTemplateQuery(IEfMockStore store) : IDocumentTemplateQuery
{
	public Task<DocumentTemplateEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<DocumentTemplateEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<DocumentTemplateEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<DocumentTemplateEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<DocumentTemplateEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
