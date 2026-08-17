using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// Write-side persistence for LearningRecommendationEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class LearningRecommendationCommand : ILearningRecommendationCommand
{
    public Task AddAsync(
        LearningRecommendationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LearningRecommendationEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        LearningRecommendationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LearningRecommendationEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        LearningRecommendationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LearningRecommendationEntity delete persistence has not been connected to the module DbContext.");
    }
}
