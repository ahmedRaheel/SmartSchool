using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// Write-side persistence for KnowledgeDocument.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class KnowledgeDocumentCommand : IKnowledgeDocumentCommand
{
    public Task AddAsync(
        KnowledgeDocument entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "KnowledgeDocument create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        KnowledgeDocument entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "KnowledgeDocument update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        KnowledgeDocument entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "KnowledgeDocument delete persistence has not been connected to the module DbContext.");
    }
}
