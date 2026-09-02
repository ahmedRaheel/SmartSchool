using SmartSchool.Modules.HR.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;

using SmartSchool.Modules.HR.Features.Interview;

namespace SmartSchool.Modules.HR.Features.DataAccess.Interview;

/// <summary>
/// Executes database writes for <see cref="InterviewEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class InterviewCommand(IHRDbContext dbContext) : IInterviewCommand
{
	public async Task AddAsync(
		InterviewEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.Interviews
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		InterviewEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Interviews
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		InterviewEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Interviews
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
