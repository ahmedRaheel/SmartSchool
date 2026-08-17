using SmartSchool.Modules.Tenancy.Models;

namespace SmartSchool.Modules.Tenancy.Persistence;

/// <summary>
/// Write-side persistence for TenantEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class TenantCommand : ITenantCommand
{
    public Task AddAsync(
        TenantEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TenantEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        TenantEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TenantEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        TenantEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TenantEntity delete persistence has not been connected to the module DbContext.");
    }
}
