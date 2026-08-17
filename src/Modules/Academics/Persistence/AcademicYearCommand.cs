using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Write-side persistence for AcademicYearEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class AcademicYearCommand : IAcademicYearCommand
{
    public Task AddAsync(
        AcademicYearEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AcademicYearEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        AcademicYearEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AcademicYearEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        AcademicYearEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AcademicYearEntity delete persistence has not been connected to the module DbContext.");
    }
}
