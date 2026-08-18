using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Audit.Models;

namespace SmartSchool.Modules.Audit.Persistence;

/// <summary>
/// EF-backed write persistence for AuditLogEntity.
/// </summary>
public sealed class AuditLogCommand(IEfMockStore store) : IAuditLogCommand
{
	public Task AddAsync(AuditLogEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(AuditLogEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(AuditLogEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
