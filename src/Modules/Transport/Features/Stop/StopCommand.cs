using SmartSchool.Modules.Transport.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Features.Stop;

/// <summary>
/// Executes database writes for <see cref="StopEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class StopCommand(ITransportDbContext dbContext) : IStopCommand
{
	public async Task AddAsync(
		StopEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.Stops
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		StopEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Stops
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		StopEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Stops
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
