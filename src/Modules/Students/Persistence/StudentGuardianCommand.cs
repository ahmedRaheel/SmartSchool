using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence;

/// <summary>
/// Write-side persistence for StudentGuardian.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class StudentGuardianCommand : IStudentGuardianCommand
{
    public Task AddAsync(
        StudentGuardian entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentGuardian create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        StudentGuardian entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentGuardian update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        StudentGuardian entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentGuardian delete persistence has not been connected to the module DbContext.");
    }
}
