using SmartSchool.Modules.Identity.Models;

namespace SmartSchool.Modules.Identity.Persistence;

/// <summary>
/// Write-side persistence for UserProfile.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class UserProfileCommand : IUserProfileCommand
{
    public Task AddAsync(
        UserProfile entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "UserProfile create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        UserProfile entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "UserProfile update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        UserProfile entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "UserProfile delete persistence has not been connected to the module DbContext.");
    }
}
