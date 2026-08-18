using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence;

/// <summary>
/// Executes database writes for <see cref="NotificationEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class NotificationCommand(IApplicationDbContext dbContext) : INotificationCommand
{
	public async Task AddAsync(
		NotificationEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<NotificationEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		NotificationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<NotificationEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		NotificationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<NotificationEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
