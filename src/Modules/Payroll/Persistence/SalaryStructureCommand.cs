using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Persistence;

/// <summary>
/// Write-side persistence for SalaryStructureEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class SalaryStructureCommand : ISalaryStructureCommand
{
    public Task AddAsync(
        SalaryStructureEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "SalaryStructureEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        SalaryStructureEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "SalaryStructureEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        SalaryStructureEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "SalaryStructureEntity delete persistence has not been connected to the module DbContext.");
    }
}
