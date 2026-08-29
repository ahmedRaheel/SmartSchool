using System.Threading.Tasks;
using SmartSchool.Modules.Tenancy.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Tenancy.Features.Tenant;

/// <summary>
/// Defines query persistence operations for TenantEntity.
/// </summary>
public interface ITenantQuery
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<TenantEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<PagedResult<TenantEntity>> GetPageAsync(		
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
