using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Identity.Models;

namespace SmartSchool.Modules.Identity.Persistence;

/// <summary>
/// Executes database writes for <see cref="RoleAssignmentEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class RoleAssignmentCommand(IApplicationDbContext dbContext) : IRoleAssignmentCommand
{
	public async Task AddAsync(
		RoleAssignmentEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<RoleAssignmentEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		RoleAssignmentEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<RoleAssignmentEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		RoleAssignmentEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<RoleAssignmentEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
