using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Write-side persistence for CourseOffering.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class CourseOfferingCommand : ICourseOfferingCommand
{
    public Task AddAsync(
        CourseOffering entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CourseOffering create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        CourseOffering entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CourseOffering update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        CourseOffering entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CourseOffering delete persistence has not been connected to the module DbContext.");
    }
}
