using SmartSchool.Modules.AIInquiry.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIInquiry.Persistence;

/// <summary>
/// Read-side persistence for InquiryMessage.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the AIInquiry module.
/// </summary>
public sealed class InquiryMessageQuery : IInquiryMessageQuery
{
    public Task<InquiryMessage?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InquiryMessage read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<InquiryMessage>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InquiryMessage paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InquiryMessage uniqueness persistence has not been connected to the module DbContext.");
    }
}
