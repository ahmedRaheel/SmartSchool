using SmartSchool.Modules.HR.Persistence;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;
namespace SmartSchool.Modules.HR.Features.Employee;
public sealed class EmployeeEvidenceWriter(IHRDbContext dbContext)
{
    public async Task AddEducationAsync(EmployeeEducationEntity entity, CancellationToken cancellationToken) { await dbContext.EmployeeEducations.AddAsync(entity, cancellationToken); await dbContext.SaveChangesAsync(cancellationToken); }
    public async Task AddExperienceAsync(EmployeeExperienceEntity entity, CancellationToken cancellationToken) { await dbContext.EmployeeExperiences.AddAsync(entity, cancellationToken); await dbContext.SaveChangesAsync(cancellationToken); }
}
