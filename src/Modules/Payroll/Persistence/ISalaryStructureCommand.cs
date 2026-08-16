using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Persistence;

public interface ISalaryStructureCommand
{
    Task AddAsync(
        SalaryStructure entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        SalaryStructure entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        SalaryStructure entity,
        CancellationToken cancellationToken);
}
