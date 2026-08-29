using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Library.Models;

namespace SmartSchool.Modules.Library.Features.Loan;

/// <summary>
/// Executes database writes for <see cref="LoanEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class LoanCommand(IApplicationDbContext dbContext) : ILoanCommand
{
	public async Task AddAsync(
		LoanEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<LoanEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		LoanEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<LoanEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		LoanEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<LoanEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
