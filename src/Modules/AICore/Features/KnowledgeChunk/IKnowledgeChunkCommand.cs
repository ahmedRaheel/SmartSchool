using System.Threading.Tasks;
using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Features.KnowledgeChunk;

/// <summary>
/// Defines command persistence operations for KnowledgeChunkEntity.
/// </summary>
public interface IKnowledgeChunkCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		KnowledgeChunkEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		KnowledgeChunkEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		KnowledgeChunkEntity entity,
		CancellationToken cancellationToken);
}
