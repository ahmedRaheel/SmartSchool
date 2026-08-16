using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

public interface ITeachingRecommendationCommand
{
    Task AddAsync(
        TeachingRecommendation entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        TeachingRecommendation entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        TeachingRecommendation entity,
        CancellationToken cancellationToken);
}
