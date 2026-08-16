using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Persistence;

/// <summary>
/// Write-side persistence for SalaryStructure.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class SalaryStructureCommand : ISalaryStructureCommand
{
    public Task AddAsync(
        SalaryStructure entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "SalaryStructure create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        SalaryStructure entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "SalaryStructure update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        SalaryStructure entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "SalaryStructure delete persistence has not been connected to the module DbContext.");
    }
}
