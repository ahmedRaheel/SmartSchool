using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Identity.Models;

namespace SmartSchool.Modules.Identity.Persistence;

/// <summary>
/// EF-backed write persistence for RoleAssignmentEntity.
/// </summary>
public sealed class RoleAssignmentCommand(IEfMockStore store) : IRoleAssignmentCommand
{
	public Task AddAsync(RoleAssignmentEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(RoleAssignmentEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(RoleAssignmentEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
