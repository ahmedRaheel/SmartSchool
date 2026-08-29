using SmartSchool.Modules.HR.Models;
namespace SmartSchool.Modules.HR.Persistence;
public interface IEmployeeEvidenceCommand
{
    Task AddEducationAsync(EmployeeEducationEntity entity, CancellationToken cancellationToken);
    Task AddExperienceAsync(EmployeeExperienceEntity entity, CancellationToken cancellationToken);
}
