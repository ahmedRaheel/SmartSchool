using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

public interface IKnowledgeCollectionCommand
{
    Task AddAsync(
        KnowledgeCollection entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        KnowledgeCollection entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        KnowledgeCollection entity,
        CancellationToken cancellationToken);
}
