using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence;

/// <summary>
/// Write-side persistence for StudentEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class StudentCommand : IStudentCommand
{
    public Task AddAsync(
        StudentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        StudentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        StudentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentEntity delete persistence has not been connected to the module DbContext.");
    }
}
