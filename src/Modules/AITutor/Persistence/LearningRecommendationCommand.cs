using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// EF-backed write persistence for LearningRecommendationEntity.
/// </summary>
public sealed class LearningRecommendationCommand(IEfMockStore store) : ILearningRecommendationCommand
{
	public Task AddAsync(LearningRecommendationEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(LearningRecommendationEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(LearningRecommendationEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
