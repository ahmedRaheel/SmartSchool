using SmartSchool.Modules.Tenancy.Models;

namespace SmartSchool.Modules.Tenancy.Persistence;

/// <summary>
/// Write-side persistence for Tenant.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class TenantCommand : ITenantCommand
{
    public Task AddAsync(
        Tenant entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Tenant create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Tenant entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Tenant update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Tenant entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Tenant delete persistence has not been connected to the module DbContext.");
    }
}
