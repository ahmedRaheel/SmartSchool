using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Write-side persistence for Candidate.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class CandidateCommand : ICandidateCommand
{
    public Task AddAsync(
        Candidate entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Candidate create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Candidate entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Candidate update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Candidate entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Candidate delete persistence has not been connected to the module DbContext.");
    }
}
