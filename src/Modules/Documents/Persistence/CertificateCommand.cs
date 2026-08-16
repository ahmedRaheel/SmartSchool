using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Persistence;

/// <summary>
/// Write-side persistence for Certificate.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class CertificateCommand : ICertificateCommand
{
    public Task AddAsync(
        Certificate entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Certificate create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Certificate entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Certificate update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Certificate entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Certificate delete persistence has not been connected to the module DbContext.");
    }
}
