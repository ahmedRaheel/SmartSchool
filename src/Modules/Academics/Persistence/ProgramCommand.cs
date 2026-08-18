using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Executes database writes for <see cref="ProgramEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ProgramCommand(IApplicationDbContext dbContext) : IProgramCommand
{
	public async Task AddAsync(
		ProgramEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<ProgramEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		ProgramEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ProgramEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		ProgramEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ProgramEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
