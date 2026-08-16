using SmartSchool.Modules.Examinations.Models;

namespace SmartSchool.Modules.Examinations.Persistence;

public interface IExamCommand
{
    Task AddAsync(
        Exam entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Exam entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Exam entity,
        CancellationToken cancellationToken);
}
