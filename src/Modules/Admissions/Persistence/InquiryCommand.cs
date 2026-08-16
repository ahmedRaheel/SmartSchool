using SmartSchool.Modules.Admissions.Models;

namespace SmartSchool.Modules.Admissions.Persistence;

/// <summary>
/// Write-side persistence for Inquiry.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class InquiryCommand : IInquiryCommand
{
    public Task AddAsync(
        Inquiry entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Inquiry create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Inquiry entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Inquiry update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Inquiry entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Inquiry delete persistence has not been connected to the module DbContext.");
    }
}
