using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Examinations.Models;

namespace SmartSchool.Modules.Examinations.Persistence;

/// <summary>
/// EF-backed write persistence for ExamEntity.
/// </summary>
public sealed class ExamCommand(IEfMockStore store) : IExamCommand
{
	public Task AddAsync(ExamEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(ExamEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(ExamEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
