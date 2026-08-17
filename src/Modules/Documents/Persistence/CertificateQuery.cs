using SmartSchool.Modules.Documents.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Persistence;

/// <summary>
/// Read-side persistence for CertificateEntity.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Documents module.
/// </summary>
public sealed class CertificateQuery : ICertificateQuery
{
    public Task<CertificateEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CertificateEntity read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<CertificateEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CertificateEntity paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CertificateEntity uniqueness persistence has not been connected to the module DbContext.");
    }
}
