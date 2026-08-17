using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence;

/// <summary>
/// Write-side persistence for StudentGuardianEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class StudentGuardianCommand : IStudentGuardianCommand
{
    public Task AddAsync(
        StudentGuardianEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentGuardianEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        StudentGuardianEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentGuardianEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        StudentGuardianEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentGuardianEntity delete persistence has not been connected to the module DbContext.");
    }
}
