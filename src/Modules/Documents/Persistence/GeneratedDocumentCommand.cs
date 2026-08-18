using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Persistence;

/// <summary>
/// Executes database writes for <see cref="GeneratedDocumentEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class GeneratedDocumentCommand(IApplicationDbContext dbContext) : IGeneratedDocumentCommand
{
	public async Task AddAsync(
		GeneratedDocumentEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<GeneratedDocumentEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		GeneratedDocumentEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<GeneratedDocumentEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		GeneratedDocumentEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<GeneratedDocumentEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
