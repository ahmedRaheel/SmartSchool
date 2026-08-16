using SmartSchool.Modules.Admissions.Models;

namespace SmartSchool.Modules.Admissions.Persistence;

/// <summary>
/// Write-side persistence for AdmissionDecision.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class AdmissionDecisionCommand : IAdmissionDecisionCommand
{
    public Task AddAsync(
        AdmissionDecision entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AdmissionDecision create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        AdmissionDecision entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AdmissionDecision update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        AdmissionDecision entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AdmissionDecision delete persistence has not been connected to the module DbContext.");
    }
}
