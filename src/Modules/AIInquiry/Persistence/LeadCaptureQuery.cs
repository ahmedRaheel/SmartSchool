using SmartSchool.Modules.AIInquiry.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIInquiry.Persistence;

/// <summary>
/// Read-side persistence for LeadCapture.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the AIInquiry module.
/// </summary>
public sealed class LeadCaptureQuery : ILeadCaptureQuery
{
    public Task<LeadCapture?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LeadCapture read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<LeadCapture>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LeadCapture paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LeadCapture uniqueness persistence has not been connected to the module DbContext.");
    }
}
