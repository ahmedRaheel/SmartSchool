using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

public interface IGradeLevelCommand
{
    Task AddAsync(
        GradeLevel entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        GradeLevel entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        GradeLevel entity,
        CancellationToken cancellationToken);
}
