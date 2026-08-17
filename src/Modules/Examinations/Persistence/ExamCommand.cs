using SmartSchool.Modules.Examinations.Models;

namespace SmartSchool.Modules.Examinations.Persistence;

/// <summary>
/// Write-side persistence for ExamEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ExamCommand : IExamCommand
{
    public Task AddAsync(
        ExamEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ExamEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ExamEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ExamEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ExamEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ExamEntity delete persistence has not been connected to the module DbContext.");
    }
}
