using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// EF-backed write persistence for StudentPerformancePredictionEntity.
/// </summary>
public sealed class StudentPerformancePredictionCommand(IEfMockStore store) : IStudentPerformancePredictionCommand
{
	public Task AddAsync(StudentPerformancePredictionEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(StudentPerformancePredictionEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(StudentPerformancePredictionEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
