using SmartSchool.Modules.Library.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Library.Models;

namespace SmartSchool.Modules.Library.Features.Reservation;

/// <summary>
/// Executes database writes for <see cref="ReservationEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ReservationCommand(ILibraryDbContext dbContext) : IReservationCommand
{
	public async Task AddAsync(
		ReservationEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.Reservations
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		ReservationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Reservations
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		ReservationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Reservations
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
