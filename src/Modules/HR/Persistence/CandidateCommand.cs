using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Write-side persistence for CandidateEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class CandidateCommand : ICandidateCommand
{
    public Task AddAsync(
        CandidateEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CandidateEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        CandidateEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CandidateEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        CandidateEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CandidateEntity delete persistence has not been connected to the module DbContext.");
    }
}
