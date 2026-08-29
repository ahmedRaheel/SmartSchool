using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Features.SchoolLogo;

/// <summary>
/// Executes database writes for <see cref="SchoolLogoEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class SchoolLogoCommand(IApplicationDbContext dbContext) : ISchoolLogoCommand
{
	public async Task AddAsync(
		SchoolLogoEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<SchoolLogoEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		SchoolLogoEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<SchoolLogoEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		SchoolLogoEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<SchoolLogoEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
