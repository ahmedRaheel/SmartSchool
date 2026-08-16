using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Write-side persistence for AcademicYear.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class AcademicYearCommand : IAcademicYearCommand
{
    public Task AddAsync(
        AcademicYear entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AcademicYear create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        AcademicYear entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AcademicYear update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        AcademicYear entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AcademicYear delete persistence has not been connected to the module DbContext.");
    }
}
