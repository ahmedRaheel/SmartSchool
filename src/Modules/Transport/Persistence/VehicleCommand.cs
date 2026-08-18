using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Persistence;

/// <summary>
/// Executes database writes for <see cref="VehicleEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class VehicleCommand(IApplicationDbContext dbContext) : IVehicleCommand
{
	public async Task AddAsync(
		VehicleEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<VehicleEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		VehicleEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<VehicleEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		VehicleEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<VehicleEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
