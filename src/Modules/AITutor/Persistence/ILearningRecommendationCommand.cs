using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

public interface ILearningRecommendationCommand
{
    Task AddAsync(
        LearningRecommendation entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        LearningRecommendation entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        LearningRecommendation entity,
        CancellationToken cancellationToken);
}
