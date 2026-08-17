using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Write-side persistence for CourseSelectionEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class CourseSelectionCommand : ICourseSelectionCommand
{
    public Task AddAsync(
        CourseSelectionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CourseSelectionEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        CourseSelectionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CourseSelectionEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        CourseSelectionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CourseSelectionEntity delete persistence has not been connected to the module DbContext.");
    }
}
