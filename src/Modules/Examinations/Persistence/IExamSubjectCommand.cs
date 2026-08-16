using SmartSchool.Modules.Examinations.Models;

namespace SmartSchool.Modules.Examinations.Persistence;

public interface IExamSubjectCommand
{
    Task AddAsync(
        ExamSubject entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        ExamSubject entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        ExamSubject entity,
        CancellationToken cancellationToken);
}
