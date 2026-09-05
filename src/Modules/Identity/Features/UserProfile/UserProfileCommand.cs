using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Identity.Models;

namespace SmartSchool.Modules.Identity.Features.UserProfile;

/// <summary>
/// Executes database writes for <see cref="UserProfileEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class UserProfileCommand(IApplicationDbContext dbContext) : IUserProfileCommand
{
    public async Task AddAsync(
        UserProfileEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext
            .Set<UserProfileEntity>()
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        UserProfileEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext
            .Set<UserProfileEntity>()
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        UserProfileEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext
            .Set<UserProfileEntity>()
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
