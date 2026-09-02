using System.Threading.Tasks;
using SmartSchool.Modules.Documents.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Features.DocumentTemplate;

/// <summary>
/// Defines query persistence operations for DocumentTemplateEntity.
/// </summary>
public interface IDocumentTemplateQuery
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<DocumentTemplateEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<PagedResult<DocumentTemplateEntity>> GetPageAsync(
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
