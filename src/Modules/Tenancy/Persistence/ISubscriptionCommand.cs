using SmartSchool.Modules.Tenancy.Models;

namespace SmartSchool.Modules.Tenancy.Persistence;

public interface ISubscriptionCommand
{
    Task AddAsync(
        Subscription entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Subscription entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Subscription entity,
        CancellationToken cancellationToken);
}
