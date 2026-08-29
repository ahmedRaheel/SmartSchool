using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Features.Subject;

/// <summary>
/// Executes database writes for <see cref="SubjectEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class SubjectCommand(IApplicationDbContext dbContext) : ISubjectCommand
{
	public async Task AddAsync(
		SubjectEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<SubjectEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		SubjectEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<SubjectEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		SubjectEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<SubjectEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
