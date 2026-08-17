using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Persistence;

/// <summary>
/// Write-side persistence for CertificateEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class CertificateCommand : ICertificateCommand
{
    public Task AddAsync(
        CertificateEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CertificateEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        CertificateEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CertificateEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        CertificateEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CertificateEntity delete persistence has not been connected to the module DbContext.");
    }
}
