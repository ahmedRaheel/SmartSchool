using SmartSchool.Modules.Examinations.Models;

namespace SmartSchool.Modules.Examinations.Persistence;

/// <summary>
/// Write-side persistence for StudentExamResult.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class StudentExamResultCommand : IStudentExamResultCommand
{
    public Task AddAsync(
        StudentExamResult entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentExamResult create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        StudentExamResult entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentExamResult update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        StudentExamResult entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentExamResult delete persistence has not been connected to the module DbContext.");
    }
}
