using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Write-side persistence for AcademicSystemEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class AcademicSystemCommand : IAcademicSystemCommand
{
    public Task AddAsync(
        AcademicSystemEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AcademicSystemEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        AcademicSystemEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AcademicSystemEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        AcademicSystemEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AcademicSystemEntity delete persistence has not been connected to the module DbContext.");
    }
}
