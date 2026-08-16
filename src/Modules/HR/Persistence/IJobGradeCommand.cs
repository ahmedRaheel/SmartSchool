using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

public interface IJobGradeCommand
{
    Task AddAsync(
        JobGrade entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        JobGrade entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        JobGrade entity,
        CancellationToken cancellationToken);
}
