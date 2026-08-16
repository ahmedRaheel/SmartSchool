using SmartSchool.Modules.Tenancy.Models;

namespace SmartSchool.Modules.Tenancy.Persistence;

/// <summary>
/// Write-side persistence for CampusBranding.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class CampusBrandingCommand : ICampusBrandingCommand
{
    public Task AddAsync(
        CampusBranding entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CampusBranding create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        CampusBranding entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CampusBranding update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        CampusBranding entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CampusBranding delete persistence has not been connected to the module DbContext.");
    }
}
