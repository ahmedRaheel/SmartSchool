using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Features.Certificate;

/// <summary>
/// Executes database writes for <see cref="CertificateEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class CertificateCommand(IApplicationDbContext dbContext) : ICertificateCommand
{
	public async Task AddAsync(
		CertificateEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<CertificateEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		CertificateEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<CertificateEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		CertificateEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<CertificateEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
