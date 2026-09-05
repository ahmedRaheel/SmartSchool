using SmartSchool.Modules.Students.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Features.Guardian;

/// <summary>
/// Executes database writes for <see cref="GuardianEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class GuardianCommand(IStudentsDbContext dbContext) : IGuardianCommand
{
    public async Task AddAsync(
        GuardianEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.Guardians
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        GuardianEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Guardians
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        GuardianEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Guardians
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
