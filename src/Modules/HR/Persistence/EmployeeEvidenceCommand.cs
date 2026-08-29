using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;
namespace SmartSchool.Modules.HR.Persistence;
public sealed class EmployeeEvidenceCommand(IApplicationDbContext dbContext) : IEmployeeEvidenceCommand
{
    public async Task AddEducationAsync(EmployeeEducationEntity entity, CancellationToken cancellationToken) { await dbContext.Set<EmployeeEducationEntity>().AddAsync(entity, cancellationToken); await dbContext.SaveChangesAsync(cancellationToken); }
    public async Task AddExperienceAsync(EmployeeExperienceEntity entity, CancellationToken cancellationToken) { await dbContext.Set<EmployeeExperienceEntity>().AddAsync(entity, cancellationToken); await dbContext.SaveChangesAsync(cancellationToken); }
}
