using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Features.Department;

/// <summary>
/// Executes database writes for <see cref="DepartmentEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class DepartmentCommand(IApplicationDbContext dbContext) : IDepartmentCommand
{
	public async Task AddAsync(
		DepartmentEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<DepartmentEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		DepartmentEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<DepartmentEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		DepartmentEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<DepartmentEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
