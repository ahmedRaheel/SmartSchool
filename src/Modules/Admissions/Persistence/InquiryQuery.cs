using SmartSchool.Modules.Admissions.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Admissions.Persistence;

/// <summary>
/// Read-side persistence for Inquiry.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Admissions module.
/// </summary>
public sealed class InquiryQuery : IInquiryQuery
{
    public Task<Inquiry?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Inquiry read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Inquiry>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Inquiry paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Inquiry uniqueness persistence has not been connected to the module DbContext.");
    }
}
