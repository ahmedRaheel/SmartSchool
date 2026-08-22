using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// Executes database writes for <see cref="PaymentEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class PaymentCommand(IApplicationDbContext dbContext) : IPaymentCommand
{
	public async Task AddAsync(
		PaymentEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<PaymentEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		PaymentEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<PaymentEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		PaymentEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<PaymentEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
