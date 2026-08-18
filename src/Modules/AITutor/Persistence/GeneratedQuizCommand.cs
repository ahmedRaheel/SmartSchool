using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// EF-backed write persistence for GeneratedQuizEntity.
/// </summary>
public sealed class GeneratedQuizCommand(IEfMockStore store) : IGeneratedQuizCommand
{
	public Task AddAsync(GeneratedQuizEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(GeneratedQuizEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(GeneratedQuizEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
