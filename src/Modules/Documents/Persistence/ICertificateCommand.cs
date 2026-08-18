using System.Threading.Tasks;
using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Persistence;

/// <summary>
/// Defines command persistence operations for CertificateEntity.
/// </summary>
public interface ICertificateCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		CertificateEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		CertificateEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		CertificateEntity entity,
		CancellationToken cancellationToken);
}
