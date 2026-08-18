using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Admissions.Models;

namespace SmartSchool.Modules.Admissions.Persistence;

/// <summary>
/// EF-backed write persistence for ApplicantEntity.
/// </summary>
public sealed class ApplicantCommand(IEfMockStore store) : IApplicantCommand
{
	public Task AddAsync(ApplicantEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(ApplicantEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(ApplicantEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
