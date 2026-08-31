using SmartSchool.Modules.HR.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Features.Position;

/// <summary>
/// Executes database writes for <see cref="PositionEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class PositionCommand(IHRDbContext dbContext) : IPositionCommand
{
	public async Task AddAsync(
		PositionEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.Positions
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		PositionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Positions
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		PositionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Positions
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
