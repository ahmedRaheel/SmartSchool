using SmartSchool.Modules.AIInquiry.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIInquiry.Persistence;

/// <summary>
/// Read-side persistence for InquiryConversation.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the AIInquiry module.
/// </summary>
public sealed class InquiryConversationQuery : IInquiryConversationQuery
{
    public Task<InquiryConversation?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InquiryConversation read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<InquiryConversation>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InquiryConversation paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InquiryConversation uniqueness persistence has not been connected to the module DbContext.");
    }
}
