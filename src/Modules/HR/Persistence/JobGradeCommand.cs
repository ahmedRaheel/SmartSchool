using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Write-side persistence for JobGradeEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class JobGradeCommand : IJobGradeCommand
{
    public Task AddAsync(
        JobGradeEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "JobGradeEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        JobGradeEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "JobGradeEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        JobGradeEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "JobGradeEntity delete persistence has not been connected to the module DbContext.");
    }
}
