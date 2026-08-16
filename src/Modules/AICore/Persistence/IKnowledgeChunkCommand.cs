using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

public interface IKnowledgeChunkCommand
{
    Task AddAsync(
        KnowledgeChunk entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        KnowledgeChunk entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        KnowledgeChunk entity,
        CancellationToken cancellationToken);
}
