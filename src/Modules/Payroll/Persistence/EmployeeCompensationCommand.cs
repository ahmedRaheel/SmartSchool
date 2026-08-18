using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Persistence;

/// <summary>
/// EF-backed write persistence for EmployeeCompensationEntity.
/// </summary>
public sealed class EmployeeCompensationCommand(IEfMockStore store) : IEmployeeCompensationCommand
{
	public Task AddAsync(EmployeeCompensationEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(EmployeeCompensationEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(EmployeeCompensationEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
