using SmartSchool.Modules.HR.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Features.Job;

/// <summary>
/// Executes database writes for <see cref="JobEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class JobCommand(IHRDbContext dbContext) : IJobCommand
{
    public async Task AddAsync(
        JobEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.Jobs
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        JobEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Jobs
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        JobEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Jobs
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
