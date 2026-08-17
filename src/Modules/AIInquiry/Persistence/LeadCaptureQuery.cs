using SmartSchool.Modules.AIInquiry.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIInquiry.Persistence;

/// <summary>
/// Read-side persistence for LeadCaptureEntity.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the AIInquiry module.
/// </summary>
public sealed class LeadCaptureQuery : ILeadCaptureQuery
{
    public Task<LeadCaptureEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LeadCaptureEntity read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<LeadCaptureEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LeadCaptureEntity paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LeadCaptureEntity uniqueness persistence has not been connected to the module DbContext.");
    }
}
