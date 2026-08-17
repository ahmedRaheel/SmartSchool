using SmartSchool.Modules.Admissions.Models;

namespace SmartSchool.Modules.Admissions.Persistence;

/// <summary>
/// Write-side persistence for InquiryEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class InquiryCommand : IInquiryCommand
{
    public Task AddAsync(
        InquiryEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InquiryEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        InquiryEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InquiryEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        InquiryEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InquiryEntity delete persistence has not been connected to the module DbContext.");
    }
}
