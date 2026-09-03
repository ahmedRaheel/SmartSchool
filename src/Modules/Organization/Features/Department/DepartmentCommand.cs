using SmartSchool.Modules.Organization.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Features.Department;

/// <summary>
/// Executes database writes for <see cref="DepartmentEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class DepartmentCommand(IOrganizationDbContext dbContext) : IDepartmentCommand
{
    public async Task AddAsync(
        DepartmentEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.Departments
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        DepartmentEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Departments
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        DepartmentEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Departments
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
