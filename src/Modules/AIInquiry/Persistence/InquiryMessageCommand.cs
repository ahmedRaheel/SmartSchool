using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Persistence;

/// <summary>
/// Write-side persistence for InquiryMessageEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class InquiryMessageCommand : IInquiryMessageCommand
{
    public Task AddAsync(
        InquiryMessageEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InquiryMessageEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        InquiryMessageEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InquiryMessageEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        InquiryMessageEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InquiryMessageEntity delete persistence has not been connected to the module DbContext.");
    }
}
