using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence;

/// <summary>
/// Write-side persistence for GuardianEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class GuardianCommand : IGuardianCommand
{
    public Task AddAsync(
        GuardianEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GuardianEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        GuardianEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GuardianEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        GuardianEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GuardianEntity delete persistence has not been connected to the module DbContext.");
    }
}
