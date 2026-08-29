using System.Threading.Tasks;
using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Features.InquiryMessage;

/// <summary>
/// Defines command persistence operations for InquiryMessageEntity.
/// </summary>
public interface IInquiryMessageCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		InquiryMessageEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		InquiryMessageEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		InquiryMessageEntity entity,
		CancellationToken cancellationToken);
}
