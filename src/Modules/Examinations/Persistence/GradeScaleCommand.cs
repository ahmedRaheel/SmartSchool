using SmartSchool.Modules.Examinations.Models;

namespace SmartSchool.Modules.Examinations.Persistence;

/// <summary>
/// Write-side persistence for GradeScaleEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class GradeScaleCommand : IGradeScaleCommand
{
    public Task AddAsync(
        GradeScaleEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GradeScaleEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        GradeScaleEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GradeScaleEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        GradeScaleEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GradeScaleEntity delete persistence has not been connected to the module DbContext.");
    }
}
