using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Write-side persistence for AcademicSystem.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class AcademicSystemCommand : IAcademicSystemCommand
{
    public Task AddAsync(
        AcademicSystem entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AcademicSystem create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        AcademicSystem entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AcademicSystem update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        AcademicSystem entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AcademicSystem delete persistence has not been connected to the module DbContext.");
    }
}
