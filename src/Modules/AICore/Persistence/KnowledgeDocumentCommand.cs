using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// Write-side persistence for KnowledgeDocumentEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class KnowledgeDocumentCommand : IKnowledgeDocumentCommand
{
    public Task AddAsync(
        KnowledgeDocumentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "KnowledgeDocumentEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        KnowledgeDocumentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "KnowledgeDocumentEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        KnowledgeDocumentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "KnowledgeDocumentEntity delete persistence has not been connected to the module DbContext.");
    }
}
