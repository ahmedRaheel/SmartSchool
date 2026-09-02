using SmartSchool.Modules.Finance.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Features.StudentFee;

/// <summary>
/// Executes database writes for <see cref="StudentFeeEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class StudentFeeCommand(IFinanceDbContext dbContext) : IStudentFeeCommand
{
	public async Task AddAsync(
		StudentFeeEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.StudentFees
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		StudentFeeEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.StudentFees
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		StudentFeeEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.StudentFees
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
