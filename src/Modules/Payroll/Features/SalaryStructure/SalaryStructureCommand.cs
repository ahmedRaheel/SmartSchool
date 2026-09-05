using SmartSchool.Modules.Payroll.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Features.SalaryStructure;

/// <summary>
/// Executes database writes for <see cref="SalaryStructureEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class SalaryStructureCommand(IPayrollDbContext dbContext) : ISalaryStructureCommand
{
    public async Task AddAsync(
        SalaryStructureEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.SalaryStructures
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        SalaryStructureEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.SalaryStructures
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        SalaryStructureEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.SalaryStructures
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
