using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Persistence;

/// <summary>
/// EF-backed write persistence for GeneratedDocumentEntity.
/// </summary>
public sealed class GeneratedDocumentCommand(IEfMockStore store) : IGeneratedDocumentCommand
{
	public Task AddAsync(GeneratedDocumentEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(GeneratedDocumentEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(GeneratedDocumentEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
