using SmartSchool.Modules.Communication.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Features.MessageReceipt;

/// <summary>
/// Executes database writes for <see cref="MessageReceiptEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class MessageReceiptCommand(ICommunicationDbContext dbContext) : IMessageReceiptCommand
{
	public async Task AddAsync(
		MessageReceiptEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.MessageReceipts
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		MessageReceiptEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.MessageReceipts
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		MessageReceiptEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.MessageReceipts
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
