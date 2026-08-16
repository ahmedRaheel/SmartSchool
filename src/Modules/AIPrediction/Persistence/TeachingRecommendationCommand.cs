using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Write-side persistence for TeachingRecommendation.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class TeachingRecommendationCommand : ITeachingRecommendationCommand
{
    public Task AddAsync(
        TeachingRecommendation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TeachingRecommendation create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        TeachingRecommendation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TeachingRecommendation update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        TeachingRecommendation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TeachingRecommendation delete persistence has not been connected to the module DbContext.");
    }
}
