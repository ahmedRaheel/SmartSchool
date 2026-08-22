using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Persistence;

/// <summary>
/// Executes database writes for <see cref="LearningResourceEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class LearningResourceCommand(IApplicationDbContext dbContext) : ILearningResourceCommand
{
	public async Task AddAsync(
		LearningResourceEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<LearningResourceEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		LearningResourceEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<LearningResourceEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		LearningResourceEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<LearningResourceEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
