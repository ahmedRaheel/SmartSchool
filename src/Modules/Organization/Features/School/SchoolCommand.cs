using SmartSchool.Modules.Organization.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Features.School;

/// <summary>
/// Executes database writes for <see cref="SchoolEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class SchoolCommand(IOrganizationDbContext dbContext) : ISchoolCommand
{
    public async Task AddAsync(
        SchoolEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.Schools
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        SchoolEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Schools
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        SchoolEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Schools
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
