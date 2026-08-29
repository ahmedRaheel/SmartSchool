using System.Threading.Tasks;
using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Features.Message;

/// <summary>
/// Defines command persistence operations for MessageEntity.
/// </summary>
public interface IMessageCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		MessageEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		MessageEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		MessageEntity entity,
		CancellationToken cancellationToken);
}
