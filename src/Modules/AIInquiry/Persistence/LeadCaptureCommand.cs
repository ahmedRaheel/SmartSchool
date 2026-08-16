using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Persistence;

/// <summary>
/// Write-side persistence for LeadCapture.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class LeadCaptureCommand : ILeadCaptureCommand
{
    public Task AddAsync(
        LeadCapture entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LeadCapture create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        LeadCapture entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LeadCapture update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        LeadCapture entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LeadCapture delete persistence has not been connected to the module DbContext.");
    }
}
