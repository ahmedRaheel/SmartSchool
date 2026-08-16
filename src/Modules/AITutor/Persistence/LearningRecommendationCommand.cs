using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// Write-side persistence for LearningRecommendation.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class LearningRecommendationCommand : ILearningRecommendationCommand
{
    public Task AddAsync(
        LearningRecommendation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LearningRecommendation create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        LearningRecommendation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LearningRecommendation update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        LearningRecommendation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LearningRecommendation delete persistence has not been connected to the module DbContext.");
    }
}
