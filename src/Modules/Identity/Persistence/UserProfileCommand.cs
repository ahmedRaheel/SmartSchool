using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Identity.Models;

namespace SmartSchool.Modules.Identity.Persistence;

/// <summary>
/// EF-backed write persistence for UserProfileEntity.
/// </summary>
public sealed class UserProfileCommand(IEfMockStore store) : IUserProfileCommand
{
	public Task AddAsync(UserProfileEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(UserProfileEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(UserProfileEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
