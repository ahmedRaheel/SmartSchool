using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// Write-side persistence for KnowledgeChunk.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class KnowledgeChunkCommand : IKnowledgeChunkCommand
{
    public Task AddAsync(
        KnowledgeChunk entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "KnowledgeChunk create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        KnowledgeChunk entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "KnowledgeChunk update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        KnowledgeChunk entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "KnowledgeChunk delete persistence has not been connected to the module DbContext.");
    }
}
