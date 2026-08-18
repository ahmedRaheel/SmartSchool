using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Tenancy.Models;

namespace SmartSchool.Modules.Tenancy.Persistence;

/// <summary>
/// EF-backed write persistence for SubscriptionEntity.
/// </summary>
public sealed class SubscriptionCommand(IEfMockStore store) : ISubscriptionCommand
{
	public Task AddAsync(SubscriptionEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(SubscriptionEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(SubscriptionEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
