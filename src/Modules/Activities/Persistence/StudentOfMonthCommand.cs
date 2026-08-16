using SmartSchool.Modules.Activities.Models;

namespace SmartSchool.Modules.Activities.Persistence;

/// <summary>
/// Write-side persistence for StudentOfMonth.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class StudentOfMonthCommand : IStudentOfMonthCommand
{
    public Task AddAsync(
        StudentOfMonth entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentOfMonth create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        StudentOfMonth entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentOfMonth update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        StudentOfMonth entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentOfMonth delete persistence has not been connected to the module DbContext.");
    }
}
