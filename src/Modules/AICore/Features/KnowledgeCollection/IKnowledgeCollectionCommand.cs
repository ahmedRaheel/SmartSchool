using System.Threading.Tasks;
using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Features.KnowledgeCollection;

/// <summary>
/// Defines command persistence operations for KnowledgeCollectionEntity.
/// </summary>
public interface IKnowledgeCollectionCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		KnowledgeCollectionEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		KnowledgeCollectionEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		KnowledgeCollectionEntity entity,
		CancellationToken cancellationToken);
}
