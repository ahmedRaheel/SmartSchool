using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence;

/// <summary>
/// Write-side persistence for Guardian.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class GuardianCommand : IGuardianCommand
{
    public Task AddAsync(
        Guardian entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Guardian create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Guardian entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Guardian update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Guardian entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Guardian delete persistence has not been connected to the module DbContext.");
    }
}
