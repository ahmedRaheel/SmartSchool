using SmartSchool.Modules.HR.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Features.Resume;

/// <summary>
/// Executes database writes for <see cref="ResumeEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ResumeCommand(IHRDbContext dbContext) : IResumeCommand
{
	public async Task AddAsync(
		ResumeEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.Resumes
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		ResumeEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Resumes
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		ResumeEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Resumes
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
