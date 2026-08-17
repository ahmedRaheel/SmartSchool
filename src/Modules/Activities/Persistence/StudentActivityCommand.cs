using SmartSchool.Modules.Activities.Models;

namespace SmartSchool.Modules.Activities.Persistence;

/// <summary>
/// Write-side persistence for StudentActivityEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class StudentActivityCommand : IStudentActivityCommand
{
    public Task AddAsync(
        StudentActivityEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentActivityEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        StudentActivityEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentActivityEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        StudentActivityEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentActivityEntity delete persistence has not been connected to the module DbContext.");
    }
}
