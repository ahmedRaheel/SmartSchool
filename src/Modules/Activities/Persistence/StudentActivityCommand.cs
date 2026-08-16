using SmartSchool.Modules.Activities.Models;

namespace SmartSchool.Modules.Activities.Persistence;

/// <summary>
/// Write-side persistence for StudentActivity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class StudentActivityCommand : IStudentActivityCommand
{
    public Task AddAsync(
        StudentActivity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentActivity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        StudentActivity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentActivity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        StudentActivity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentActivity delete persistence has not been connected to the module DbContext.");
    }
}
