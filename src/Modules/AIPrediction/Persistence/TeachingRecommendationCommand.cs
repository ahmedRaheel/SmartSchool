using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Write-side persistence for TeachingRecommendationEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class TeachingRecommendationCommand : ITeachingRecommendationCommand
{
    public Task AddAsync(
        TeachingRecommendationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TeachingRecommendationEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        TeachingRecommendationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TeachingRecommendationEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        TeachingRecommendationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TeachingRecommendationEntity delete persistence has not been connected to the module DbContext.");
    }
}
