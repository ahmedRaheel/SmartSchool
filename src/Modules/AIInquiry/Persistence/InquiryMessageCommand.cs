using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Persistence;

/// <summary>
/// Write-side persistence for InquiryMessage.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class InquiryMessageCommand : IInquiryMessageCommand
{
    public Task AddAsync(
        InquiryMessage entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InquiryMessage create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        InquiryMessage entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InquiryMessage update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        InquiryMessage entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InquiryMessage delete persistence has not been connected to the module DbContext.");
    }
}
