using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Admissions.Models;

namespace SmartSchool.Modules.Admissions.Persistence;

/// <summary>
/// Executes database writes for <see cref="AdmissionDecisionEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class AdmissionDecisionCommand(IApplicationDbContext dbContext) : IAdmissionDecisionCommand
{
	public async Task AddAsync(
		AdmissionDecisionEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<AdmissionDecisionEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		AdmissionDecisionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<AdmissionDecisionEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		AdmissionDecisionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<AdmissionDecisionEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
