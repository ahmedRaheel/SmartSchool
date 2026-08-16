using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Persistence;

/// <summary>
/// Write-side persistence for InquiryConversation.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class InquiryConversationCommand : IInquiryConversationCommand
{
    public Task AddAsync(
        InquiryConversation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InquiryConversation create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        InquiryConversation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InquiryConversation update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        InquiryConversation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InquiryConversation delete persistence has not been connected to the module DbContext.");
    }
}
