using SmartSchool.Modules.AICore.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Features.KnowledgeChunk;

/// <summary>
/// Executes database writes for <see cref="KnowledgeChunkEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class KnowledgeChunkCommand(IAICoreDbContext dbContext) : IKnowledgeChunkCommand
{
    public async Task AddAsync(
        KnowledgeChunkEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.KnowledgeChunks
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        KnowledgeChunkEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.KnowledgeChunks
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        KnowledgeChunkEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.KnowledgeChunks
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
