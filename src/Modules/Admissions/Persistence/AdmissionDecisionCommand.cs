using SmartSchool.Modules.Admissions.Models;

namespace SmartSchool.Modules.Admissions.Persistence;

/// <summary>
/// Write-side persistence for AdmissionDecisionEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class AdmissionDecisionCommand : IAdmissionDecisionCommand
{
    public Task AddAsync(
        AdmissionDecisionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AdmissionDecisionEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        AdmissionDecisionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AdmissionDecisionEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        AdmissionDecisionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AdmissionDecisionEntity delete persistence has not been connected to the module DbContext.");
    }
}
