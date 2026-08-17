using SmartSchool.Modules.Examinations.Models;

namespace SmartSchool.Modules.Examinations.Persistence;

/// <summary>
/// Write-side persistence for StudentExamResultEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class StudentExamResultCommand : IStudentExamResultCommand
{
    public Task AddAsync(
        StudentExamResultEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentExamResultEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        StudentExamResultEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentExamResultEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        StudentExamResultEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentExamResultEntity delete persistence has not been connected to the module DbContext.");
    }
}
