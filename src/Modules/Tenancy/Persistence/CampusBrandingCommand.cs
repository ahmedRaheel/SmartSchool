using SmartSchool.Modules.Tenancy.Models;

namespace SmartSchool.Modules.Tenancy.Persistence;

/// <summary>
/// Write-side persistence for CampusBrandingEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class CampusBrandingCommand : ICampusBrandingCommand
{
    public Task AddAsync(
        CampusBrandingEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CampusBrandingEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        CampusBrandingEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CampusBrandingEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        CampusBrandingEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CampusBrandingEntity delete persistence has not been connected to the module DbContext.");
    }
}
