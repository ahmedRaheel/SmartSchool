using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Persistence;

/// <summary>
/// EF-backed write persistence for DocumentTemplateEntity.
/// </summary>
public sealed class DocumentTemplateCommand(IEfMockStore store) : IDocumentTemplateCommand
{
	public Task AddAsync(DocumentTemplateEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(DocumentTemplateEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(DocumentTemplateEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
