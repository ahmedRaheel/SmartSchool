using SmartSchool.Modules.Tenancy.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Tenancy.Models;

namespace SmartSchool.Modules.Tenancy.Features.Tenant;

/// <summary>
/// Executes database writes for <see cref="TenantEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class TenantCommand(ITenancyDbContext dbContext) : ITenantCommand
{
	public async Task AddAsync(
		TenantEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.Tenants
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		TenantEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Tenants
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		TenantEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Tenants
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
