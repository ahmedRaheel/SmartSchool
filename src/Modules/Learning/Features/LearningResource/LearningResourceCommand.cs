using SmartSchool.Modules.Learning.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Features.LearningResource;

/// <summary>
/// Executes database writes for <see cref="LearningResourceEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class LearningResourceCommand(ILearningDbContext dbContext) : ILearningResourceCommand
{
    public async Task AddAsync(
        LearningResourceEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.LearningResources
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        LearningResourceEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.LearningResources
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        LearningResourceEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.LearningResources
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
