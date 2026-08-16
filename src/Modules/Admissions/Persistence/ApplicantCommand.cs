using SmartSchool.Modules.Admissions.Models;

namespace SmartSchool.Modules.Admissions.Persistence;

/// <summary>
/// Write-side persistence for Applicant.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ApplicantCommand : IApplicantCommand
{
    public Task AddAsync(
        Applicant entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Applicant create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Applicant entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Applicant update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Applicant entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Applicant delete persistence has not been connected to the module DbContext.");
    }
}
