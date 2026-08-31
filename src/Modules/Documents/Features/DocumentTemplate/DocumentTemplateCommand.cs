using SmartSchool.Modules.Documents.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Features.DocumentTemplate;

/// <summary>
/// Executes database writes for <see cref="DocumentTemplateEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class DocumentTemplateCommand(IDocumentsDbContext dbContext) : IDocumentTemplateCommand
{
	public async Task AddAsync(
		DocumentTemplateEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.DocumentTemplates
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		DocumentTemplateEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.DocumentTemplates
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		DocumentTemplateEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.DocumentTemplates
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
