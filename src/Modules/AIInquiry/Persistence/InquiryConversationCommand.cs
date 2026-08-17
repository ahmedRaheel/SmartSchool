using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Persistence;

/// <summary>
/// Write-side persistence for InquiryConversationEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class InquiryConversationCommand : IInquiryConversationCommand
{
    public Task AddAsync(
        InquiryConversationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InquiryConversationEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        InquiryConversationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InquiryConversationEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        InquiryConversationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InquiryConversationEntity delete persistence has not been connected to the module DbContext.");
    }
}
