using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Activities.Models;

namespace SmartSchool.Modules.Activities.Persistence;

/// <summary>
/// EF-backed write persistence for ActivityEntity.
/// </summary>
public sealed class ActivityCommand(IEfMockStore store) : IActivityCommand
{
	public Task AddAsync(ActivityEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(ActivityEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(ActivityEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
