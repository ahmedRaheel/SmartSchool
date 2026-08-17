using SmartSchool.Modules.Admissions.Models;

namespace SmartSchool.Modules.Admissions.Persistence;

/// <summary>
/// Write-side persistence for ApplicantEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ApplicantCommand : IApplicantCommand
{
    public Task AddAsync(
        ApplicantEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ApplicantEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ApplicantEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ApplicantEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ApplicantEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ApplicantEntity delete persistence has not been connected to the module DbContext.");
    }
}
