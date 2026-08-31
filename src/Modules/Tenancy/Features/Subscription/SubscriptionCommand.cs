using SmartSchool.Modules.Tenancy.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Tenancy.Models;

namespace SmartSchool.Modules.Tenancy.Features.Subscription;

/// <summary>
/// Executes database writes for <see cref="SubscriptionEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class SubscriptionCommand(ITenancyDbContext dbContext) : ISubscriptionCommand
{
	public async Task AddAsync(
		SubscriptionEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.Subscriptions
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		SubscriptionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Subscriptions
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		SubscriptionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Subscriptions
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
