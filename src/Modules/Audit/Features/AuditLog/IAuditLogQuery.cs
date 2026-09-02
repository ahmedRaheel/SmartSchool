using System.Threading.Tasks;
using SmartSchool.Modules.Audit.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Audit.Features.AuditLog;

/// <summary>
/// Defines query persistence operations for AuditLogEntity.
/// </summary>
public interface IAuditLogQuery
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<AuditLogEntity?> GetByIdAsync(
		Guid tenantId,
		long id,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<PagedResult<AuditLogEntity>> GetPageAsync(
		Guid tenantId,
		int page,
		int pageSize,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<bool> ExistsByCodeAsync(
		Guid tenantId,
		string code,
		Guid? excludingId,
		CancellationToken cancellationToken);
}
