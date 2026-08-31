using SmartSchool.Modules.Finance.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Features.Payment;

/// <summary>
/// Executes database writes for <see cref="PaymentEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class PaymentCommand(IFinanceDbContext dbContext) : IPaymentCommand
{
	public async Task AddAsync(
		PaymentEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.Payments
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		PaymentEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Payments
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		PaymentEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Payments
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
