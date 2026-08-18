using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIParent.Models;

namespace SmartSchool.Modules.AIParent.Persistence;

/// <summary>
/// EF-backed write persistence for ParentToolExecutionEntity.
/// </summary>
public sealed class ParentToolExecutionCommand(IEfMockStore store) : IParentToolExecutionCommand
{
	public Task AddAsync(ParentToolExecutionEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(ParentToolExecutionEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(ParentToolExecutionEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
