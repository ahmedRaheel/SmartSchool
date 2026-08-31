using SmartSchool.Modules.Documents.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Features.GeneratedDocument;

/// <summary>
/// Executes database writes for <see cref="GeneratedDocumentEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class GeneratedDocumentCommand(IDocumentsDbContext dbContext) : IGeneratedDocumentCommand
{
	public async Task AddAsync(
		GeneratedDocumentEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.GeneratedDocuments
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		GeneratedDocumentEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.GeneratedDocuments
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		GeneratedDocumentEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.GeneratedDocuments
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
