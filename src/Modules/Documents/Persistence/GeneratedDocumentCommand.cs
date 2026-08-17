using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Persistence;

/// <summary>
/// Write-side persistence for GeneratedDocumentEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class GeneratedDocumentCommand : IGeneratedDocumentCommand
{
    public Task AddAsync(
        GeneratedDocumentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GeneratedDocumentEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        GeneratedDocumentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GeneratedDocumentEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        GeneratedDocumentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GeneratedDocumentEntity delete persistence has not been connected to the module DbContext.");
    }
}
