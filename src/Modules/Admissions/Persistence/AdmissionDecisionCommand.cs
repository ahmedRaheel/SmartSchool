using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Admissions.Models;

namespace SmartSchool.Modules.Admissions.Persistence;

/// <summary>
/// EF-backed write persistence for AdmissionDecisionEntity.
/// </summary>
public sealed class AdmissionDecisionCommand(IEfMockStore store) : IAdmissionDecisionCommand
{
	public Task AddAsync(AdmissionDecisionEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(AdmissionDecisionEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(AdmissionDecisionEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
