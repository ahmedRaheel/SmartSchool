using SmartSchool.Modules.Examinations.Models;

namespace SmartSchool.Modules.Examinations.Persistence;

/// <summary>
/// Write-side persistence for ExamSubject.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ExamSubjectCommand : IExamSubjectCommand
{
    public Task AddAsync(
        ExamSubject entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ExamSubject create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ExamSubject entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ExamSubject update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ExamSubject entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ExamSubject delete persistence has not been connected to the module DbContext.");
    }
}
