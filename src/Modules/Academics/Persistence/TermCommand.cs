using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Executes database writes for <see cref="TermEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class TermCommand(IApplicationDbContext dbContext) : ITermCommand
{
	public async Task AddAsync(
		TermEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<TermEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		TermEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<TermEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		TermEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<TermEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
