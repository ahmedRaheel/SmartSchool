using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// Executes database writes for <see cref="InvoiceEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class InvoiceCommand(IApplicationDbContext dbContext) : IInvoiceCommand
{
	public async Task AddAsync(
		InvoiceEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<InvoiceEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		InvoiceEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<InvoiceEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		InvoiceEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<InvoiceEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
