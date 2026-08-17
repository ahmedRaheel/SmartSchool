using SmartSchool.Modules.Identity.Models;

namespace SmartSchool.Modules.Identity.Persistence;

/// <summary>
/// Write-side persistence for UserProfileEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class UserProfileCommand : IUserProfileCommand
{
    public Task AddAsync(
        UserProfileEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "UserProfileEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        UserProfileEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "UserProfileEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        UserProfileEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "UserProfileEntity delete persistence has not been connected to the module DbContext.");
    }
}
