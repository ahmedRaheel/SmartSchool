using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Persistence;

/// <summary>
/// Write-side persistence for DocumentTemplateEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class DocumentTemplateCommand : IDocumentTemplateCommand
{
    public Task AddAsync(
        DocumentTemplateEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "DocumentTemplateEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        DocumentTemplateEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "DocumentTemplateEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        DocumentTemplateEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "DocumentTemplateEntity delete persistence has not been connected to the module DbContext.");
    }
}
