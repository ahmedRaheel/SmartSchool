using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Write-side persistence for CourseSelection.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class CourseSelectionCommand : ICourseSelectionCommand
{
    public Task AddAsync(
        CourseSelection entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CourseSelection create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        CourseSelection entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CourseSelection update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        CourseSelection entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CourseSelection delete persistence has not been connected to the module DbContext.");
    }
}
