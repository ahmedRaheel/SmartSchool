using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Write-side persistence for CourseOfferingEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class CourseOfferingCommand : ICourseOfferingCommand
{
    public Task AddAsync(
        CourseOfferingEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CourseOfferingEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        CourseOfferingEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CourseOfferingEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        CourseOfferingEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CourseOfferingEntity delete persistence has not been connected to the module DbContext.");
    }
}
