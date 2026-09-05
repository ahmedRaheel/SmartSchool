using SmartSchool.Modules.AITutor.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Features.LearningRecommendation;

/// <summary>
/// Executes database writes for <see cref="LearningRecommendationEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class LearningRecommendationCommand(IAITutorDbContext dbContext) : ILearningRecommendationCommand
{
    public async Task AddAsync(
        LearningRecommendationEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.LearningRecommendations
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        LearningRecommendationEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.LearningRecommendations
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        LearningRecommendationEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.LearningRecommendations
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
