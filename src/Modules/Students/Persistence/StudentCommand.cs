using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence;

/// <summary>
/// Write-side persistence for Student.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class StudentCommand : IStudentCommand
{
    public Task AddAsync(
        Student entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Student create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Student entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Student update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Student entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Student delete persistence has not been connected to the module DbContext.");
    }
}
