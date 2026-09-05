using SmartSchool.Modules.Payroll.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Features.EmployeeCompensation;

/// <summary>
/// Executes database writes for <see cref="EmployeeCompensationEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class EmployeeCompensationCommand(IPayrollDbContext dbContext) : IEmployeeCompensationCommand
{
    public async Task AddAsync(
        EmployeeCompensationEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.EmployeeCompensations
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        EmployeeCompensationEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.EmployeeCompensations
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        EmployeeCompensationEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.EmployeeCompensations
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
