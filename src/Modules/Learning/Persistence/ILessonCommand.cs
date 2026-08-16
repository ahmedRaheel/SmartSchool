using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Persistence;

public interface ILessonCommand
{
    Task AddAsync(
        Lesson entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Lesson entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Lesson entity,
        CancellationToken cancellationToken);
}
