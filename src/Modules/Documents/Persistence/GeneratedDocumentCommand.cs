using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Persistence;

/// <summary>
/// Write-side persistence for GeneratedDocument.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class GeneratedDocumentCommand : IGeneratedDocumentCommand
{
    public Task AddAsync(
        GeneratedDocument entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GeneratedDocument create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        GeneratedDocument entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GeneratedDocument update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        GeneratedDocument entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GeneratedDocument delete persistence has not been connected to the module DbContext.");
    }
}
