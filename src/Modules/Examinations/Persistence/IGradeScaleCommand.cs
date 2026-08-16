using SmartSchool.Modules.Examinations.Models;

namespace SmartSchool.Modules.Examinations.Persistence;

public interface IGradeScaleCommand
{
    Task AddAsync(
        GradeScale entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        GradeScale entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        GradeScale entity,
        CancellationToken cancellationToken);
}
