using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Persistence;

public interface IEmployeeCompensationCommand
{
    Task AddAsync(
        EmployeeCompensation entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        EmployeeCompensation entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        EmployeeCompensation entity,
        CancellationToken cancellationToken);
}
