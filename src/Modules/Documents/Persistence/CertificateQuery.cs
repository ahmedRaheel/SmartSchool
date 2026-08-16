using SmartSchool.Modules.Documents.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Persistence;

/// <summary>
/// Read-side persistence for Certificate.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Documents module.
/// </summary>
public sealed class CertificateQuery : ICertificateQuery
{
    public Task<Certificate?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Certificate read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Certificate>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Certificate paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Certificate uniqueness persistence has not been connected to the module DbContext.");
    }
}
