using SmartSchool.Modules.Examinations.Models;

namespace SmartSchool.Modules.Examinations.Persistence;

public interface IStudentExamResultCommand
{
    Task AddAsync(
        StudentExamResult entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        StudentExamResult entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        StudentExamResult entity,
        CancellationToken cancellationToken);
}
