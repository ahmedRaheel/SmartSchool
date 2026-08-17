using SmartSchool.Modules.Activities.Models;

namespace SmartSchool.Modules.Activities.Persistence;

/// <summary>
/// Write-side persistence for StudentOfMonthEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class StudentOfMonthCommand : IStudentOfMonthCommand
{
    public Task AddAsync(
        StudentOfMonthEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentOfMonthEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        StudentOfMonthEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentOfMonthEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        StudentOfMonthEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentOfMonthEntity delete persistence has not been connected to the module DbContext.");
    }
}
