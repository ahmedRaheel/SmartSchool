using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Persistence;

/// <summary>
/// Write-side persistence for DocumentTemplate.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class DocumentTemplateCommand : IDocumentTemplateCommand
{
    public Task AddAsync(
        DocumentTemplate entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "DocumentTemplate create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        DocumentTemplate entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "DocumentTemplate update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        DocumentTemplate entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "DocumentTemplate delete persistence has not been connected to the module DbContext.");
    }
}
