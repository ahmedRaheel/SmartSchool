using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

public interface IKnowledgeDocumentCommand
{
    Task AddAsync(
        KnowledgeDocument entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        KnowledgeDocument entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        KnowledgeDocument entity,
        CancellationToken cancellationToken);
}
