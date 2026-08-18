using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// EF-backed write persistence for TutorConversationEntity.
/// </summary>
public sealed class TutorConversationCommand(IEfMockStore store) : ITutorConversationCommand
{
	public Task AddAsync(TutorConversationEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(TutorConversationEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(TutorConversationEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
