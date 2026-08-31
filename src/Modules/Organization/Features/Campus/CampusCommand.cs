using SmartSchool.Modules.Organization.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Features.Campus;

/// <summary>
/// Executes database writes for <see cref="CampusEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class CampusCommand(IOrganizationDbContext dbContext) : ICampusCommand
{
	public async Task AddAsync(
		CampusEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.Campuses
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		CampusEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Campuses
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		CampusEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Campuses
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
