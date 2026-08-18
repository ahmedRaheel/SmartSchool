using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Activities.Models;

namespace SmartSchool.Modules.Activities.Persistence;

/// <summary>
/// EF-backed write persistence for StudentOfMonthEntity.
/// </summary>
public sealed class StudentOfMonthCommand(IEfMockStore store) : IStudentOfMonthCommand
{
	public Task AddAsync(StudentOfMonthEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(StudentOfMonthEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(StudentOfMonthEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
