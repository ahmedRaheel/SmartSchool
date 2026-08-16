using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Persistence;

/// <summary>
/// Write-side persistence for Lesson.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class LessonCommand : ILessonCommand
{
    public Task AddAsync(
        Lesson entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Lesson create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Lesson entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Lesson update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Lesson entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Lesson delete persistence has not been connected to the module DbContext.");
    }
}
