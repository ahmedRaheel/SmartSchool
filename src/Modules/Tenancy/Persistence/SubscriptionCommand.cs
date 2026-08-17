using SmartSchool.Modules.Tenancy.Models;

namespace SmartSchool.Modules.Tenancy.Persistence;

/// <summary>
/// Write-side persistence for SubscriptionEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class SubscriptionCommand : ISubscriptionCommand
{
    public Task AddAsync(
        SubscriptionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "SubscriptionEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        SubscriptionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "SubscriptionEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        SubscriptionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "SubscriptionEntity delete persistence has not been connected to the module DbContext.");
    }
}
