using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// EF-backed write persistence for AiExecutionLogEntity.
/// </summary>
public sealed class AiExecutionLogCommand(IEfMockStore store) : IAiExecutionLogCommand
{
	public Task AddAsync(AiExecutionLogEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(AiExecutionLogEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(AiExecutionLogEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
