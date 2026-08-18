using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Tenancy.Models;

namespace SmartSchool.Modules.Tenancy.Persistence;

/// <summary>
/// Executes database writes for <see cref="TenantEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class TenantCommand(IApplicationDbContext dbContext) : ITenantCommand
{
	public async Task AddAsync(
		TenantEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<TenantEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		TenantEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<TenantEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		TenantEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<TenantEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
