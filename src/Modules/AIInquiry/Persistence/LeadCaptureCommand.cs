using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Persistence;

/// <summary>
/// Write-side persistence for LeadCaptureEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class LeadCaptureCommand : ILeadCaptureCommand
{
    public Task AddAsync(
        LeadCaptureEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LeadCaptureEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        LeadCaptureEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LeadCaptureEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        LeadCaptureEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LeadCaptureEntity delete persistence has not been connected to the module DbContext.");
    }
}
