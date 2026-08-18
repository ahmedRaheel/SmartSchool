using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Persistence;

/// <summary>
/// EF-backed write persistence for AssignmentSubmissionEntity.
/// </summary>
public sealed class AssignmentSubmissionCommand(IEfMockStore store) : IAssignmentSubmissionCommand
{
	public Task AddAsync(AssignmentSubmissionEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(AssignmentSubmissionEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(AssignmentSubmissionEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
