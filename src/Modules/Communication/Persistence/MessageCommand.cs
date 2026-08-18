using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence;

/// <summary>
/// Executes database writes for <see cref="MessageEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class MessageCommand(IApplicationDbContext dbContext) : IMessageCommand
{
	public async Task AddAsync(
		MessageEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<MessageEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		MessageEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<MessageEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		MessageEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<MessageEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
