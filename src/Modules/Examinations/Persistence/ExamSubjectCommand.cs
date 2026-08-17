using SmartSchool.Modules.Examinations.Models;

namespace SmartSchool.Modules.Examinations.Persistence;

/// <summary>
/// Write-side persistence for ExamSubjectEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ExamSubjectCommand : IExamSubjectCommand
{
    public Task AddAsync(
        ExamSubjectEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ExamSubjectEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ExamSubjectEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ExamSubjectEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ExamSubjectEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ExamSubjectEntity delete persistence has not been connected to the module DbContext.");
    }
}
