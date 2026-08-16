using SmartSchool.Modules.Identity.Models;

namespace SmartSchool.Modules.Identity.Persistence;

public interface IUserProfileCommand
{
    Task AddAsync(
        UserProfile entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        UserProfile entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        UserProfile entity,
        CancellationToken cancellationToken);
}
