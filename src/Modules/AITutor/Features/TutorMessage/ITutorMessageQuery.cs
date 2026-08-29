using System.Threading.Tasks;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AITutor.Features.TutorMessage;

/// <summary>
/// Defines query persistence operations for TutorMessageEntity.
/// </summary>
public interface ITutorMessageQuery
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<TutorMessageEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<PagedResult<TutorMessageEntity>> GetPageAsync(
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
