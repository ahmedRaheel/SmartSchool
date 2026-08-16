using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Persistence;

/// <summary>
/// Write-side persistence for Campus.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class CampusCommand : ICampusCommand
{
    public Task AddAsync(
        Campus entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Campus create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Campus entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Campus update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Campus entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Campus delete persistence has not been connected to the module DbContext.");
    }
}
