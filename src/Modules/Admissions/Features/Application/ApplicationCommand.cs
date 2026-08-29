using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Admissions.Models;

namespace SmartSchool.Modules.Admissions.Features.Application;

/// <summary>
/// Executes database writes for <see cref="ApplicationEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ApplicationCommand(IApplicationDbContext dbContext) : IApplicationCommand
{
	public async Task AddAsync(
		ApplicationEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<ApplicationEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		ApplicationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ApplicationEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		ApplicationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ApplicationEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
