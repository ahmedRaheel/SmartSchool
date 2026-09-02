using System.Threading.Tasks;
using SmartSchool.Modules.Admissions.Models;

namespace SmartSchool.Modules.Admissions.Features.Inquiry;

/// <summary>
/// Defines command persistence operations for InquiryEntity.
/// </summary>
public interface IInquiryCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		InquiryEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		InquiryEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		InquiryEntity entity,
		CancellationToken cancellationToken);
}
