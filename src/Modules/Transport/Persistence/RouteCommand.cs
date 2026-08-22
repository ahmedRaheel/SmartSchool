using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Persistence;

/// <summary>
/// Executes database writes for <see cref="RouteEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class RouteCommand(IApplicationDbContext dbContext) : IRouteCommand
{
	public async Task AddAsync(
		RouteEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<RouteEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		RouteEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<RouteEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		RouteEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<RouteEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
