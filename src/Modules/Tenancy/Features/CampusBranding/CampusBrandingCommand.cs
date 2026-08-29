using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Tenancy.Models;

namespace SmartSchool.Modules.Tenancy.Features.CampusBranding;

/// <summary>
/// Executes database writes for <see cref="CampusBrandingEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class CampusBrandingCommand(IApplicationDbContext dbContext) : ICampusBrandingCommand
{
	public async Task AddAsync(
		CampusBrandingEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<CampusBrandingEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		CampusBrandingEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<CampusBrandingEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		CampusBrandingEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<CampusBrandingEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
