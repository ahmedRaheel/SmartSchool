using SmartSchool.Modules.Finance.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Features.Scholarship;

/// <summary>
/// Executes database writes for <see cref="ScholarshipEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ScholarshipCommand(IFinanceDbContext dbContext) : IScholarshipCommand
{
	public async Task AddAsync(
		ScholarshipEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.Scholarships
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		ScholarshipEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Scholarships
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		ScholarshipEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Scholarships
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
