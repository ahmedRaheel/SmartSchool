using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Write-side persistence for JobGrade.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class JobGradeCommand : IJobGradeCommand
{
    public Task AddAsync(
        JobGrade entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "JobGrade create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        JobGrade entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "JobGrade update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        JobGrade entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "JobGrade delete persistence has not been connected to the module DbContext.");
    }
}
