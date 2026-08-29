using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Features.StudentPerformancePrediction;

/// <summary>
/// Executes database writes for <see cref="StudentPerformancePredictionEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class StudentPerformancePredictionCommand(IApplicationDbContext dbContext) : IStudentPerformancePredictionCommand
{
	public async Task AddAsync(
		StudentPerformancePredictionEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<StudentPerformancePredictionEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		StudentPerformancePredictionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<StudentPerformancePredictionEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		StudentPerformancePredictionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<StudentPerformancePredictionEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
