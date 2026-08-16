using SmartSchool.Modules.Activities.Models;

namespace SmartSchool.Modules.Activities.Persistence;

public interface IStudentOfMonthCommand
{
    Task AddAsync(
        StudentOfMonth entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        StudentOfMonth entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        StudentOfMonth entity,
        CancellationToken cancellationToken);
}
