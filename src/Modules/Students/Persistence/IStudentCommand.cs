using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence;

public interface IStudentCommand
{
    Task AddAsync(
        Student entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Student entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Student entity,
        CancellationToken cancellationToken);
}
