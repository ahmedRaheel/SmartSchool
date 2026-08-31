using SmartSchool.Modules.Tenancy.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Tenancy.Models;

namespace SmartSchool.Modules.Tenancy.Features.CampusBranding;

/// <summary>
/// Executes database writes for <see cref="CampusBrandingEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class CampusBrandingCommand(ITenancyDbContext dbContext) : ICampusBrandingCommand
{
	public async Task AddAsync(
		CampusBrandingEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.CampusBrandings
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		CampusBrandingEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.CampusBrandings
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		CampusBrandingEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.CampusBrandings
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
