using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

public interface ICourseSelectionCommand
{
    Task AddAsync(
        CourseSelection entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        CourseSelection entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        CourseSelection entity,
        CancellationToken cancellationToken);
}
