using System.Threading.Tasks;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AICore.Features.ModelConfiguration;

/// <summary>
/// Defines query persistence operations for ModelConfigurationEntity.
/// </summary>
public interface IModelConfigurationQuery
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<ModelConfigurationEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<PagedResult<ModelConfigurationEntity>> GetPageAsync(
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
