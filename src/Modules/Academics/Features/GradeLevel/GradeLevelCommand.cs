using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Features.GradeLevel;

/// <summary>
/// Executes database writes for <see cref="GradeLevelEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class GradeLevelCommand(IApplicationDbContext dbContext) : IGradeLevelCommand
{
	public async Task AddAsync(
		GradeLevelEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<GradeLevelEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		GradeLevelEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<GradeLevelEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		GradeLevelEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<GradeLevelEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
