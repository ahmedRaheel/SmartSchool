using SmartSchool.Modules.AICore.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Features.KnowledgeDocument;

/// <summary>
/// Executes database writes for <see cref="KnowledgeDocumentEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class KnowledgeDocumentCommand(IAICoreDbContext dbContext) : IKnowledgeDocumentCommand
{
    public async Task AddAsync(
        KnowledgeDocumentEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.KnowledgeDocuments
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        KnowledgeDocumentEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.KnowledgeDocuments
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        KnowledgeDocumentEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.KnowledgeDocuments
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
