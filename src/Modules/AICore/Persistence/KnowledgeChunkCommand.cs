using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// Write-side persistence for KnowledgeChunkEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class KnowledgeChunkCommand : IKnowledgeChunkCommand
{
    public Task AddAsync(
        KnowledgeChunkEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "KnowledgeChunkEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        KnowledgeChunkEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "KnowledgeChunkEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        KnowledgeChunkEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "KnowledgeChunkEntity delete persistence has not been connected to the module DbContext.");
    }
}
