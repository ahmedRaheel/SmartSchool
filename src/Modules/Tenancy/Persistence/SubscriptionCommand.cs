using SmartSchool.Modules.Tenancy.Models;

namespace SmartSchool.Modules.Tenancy.Persistence;

/// <summary>
/// Write-side persistence for Subscription.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class SubscriptionCommand : ISubscriptionCommand
{
    public Task AddAsync(
        Subscription entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Subscription create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Subscription entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Subscription update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Subscription entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Subscription delete persistence has not been connected to the module DbContext.");
    }
}
