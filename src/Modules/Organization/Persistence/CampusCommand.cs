using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Persistence;

/// <summary>
/// Write-side persistence for CampusEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class CampusCommand : ICampusCommand
{
    public Task AddAsync(
        CampusEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CampusEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        CampusEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CampusEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        CampusEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CampusEntity delete persistence has not been connected to the module DbContext.");
    }
}
