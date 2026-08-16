using SmartSchool.Modules.Examinations.Models;

namespace SmartSchool.Modules.Examinations.Persistence;

/// <summary>
/// Write-side persistence for Exam.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ExamCommand : IExamCommand
{
    public Task AddAsync(
        Exam entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Exam create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Exam entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Exam update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Exam entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Exam delete persistence has not been connected to the module DbContext.");
    }
}
