using SmartSchool.Modules.Audit.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Audit.Models;

namespace SmartSchool.Modules.Audit.Features.AuditLog;

/// <summary>
/// Executes database writes for <see cref="AuditLogEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class AuditLogCommand(IAuditDbContext dbContext) : IAuditLogCommand
{
	public async Task AddAsync(
		AuditLogEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.AuditLogs
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		AuditLogEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.AuditLogs
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		AuditLogEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.AuditLogs
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
