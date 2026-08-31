using SmartSchool.Modules.HR.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Features.EmploymentHistory;

/// <summary>
/// Executes database writes for <see cref="EmploymentHistoryEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class EmploymentHistoryCommand(IHRDbContext dbContext) : IEmploymentHistoryCommand
{
	public async Task AddAsync(
		EmploymentHistoryEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.EmploymentHistories
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		EmploymentHistoryEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.EmploymentHistories
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		EmploymentHistoryEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.EmploymentHistories
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
