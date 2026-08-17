using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Persistence;

/// <summary>
/// Write-side persistence for LessonEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class LessonCommand : ILessonCommand
{
    public Task AddAsync(
        LessonEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LessonEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        LessonEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LessonEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        LessonEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LessonEntity delete persistence has not been connected to the module DbContext.");
    }
}
