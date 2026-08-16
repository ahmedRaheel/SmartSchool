using SmartSchool.Modules.Examinations.Models;

namespace SmartSchool.Modules.Examinations.Persistence;

/// <summary>
/// Write-side persistence for GradeScale.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class GradeScaleCommand : IGradeScaleCommand
{
    public Task AddAsync(
        GradeScale entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GradeScale create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        GradeScale entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GradeScale update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        GradeScale entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GradeScale delete persistence has not been connected to the module DbContext.");
    }
}
