using System.Threading.Tasks;
using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Features.Conversation;

/// <summary>
/// Defines command persistence operations for ConversationEntity.
/// </summary>
public interface IConversationCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		ConversationEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		ConversationEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		ConversationEntity entity,
		CancellationToken cancellationToken);
}
