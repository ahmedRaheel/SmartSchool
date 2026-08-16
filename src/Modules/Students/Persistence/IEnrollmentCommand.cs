using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence;

public interface IEnrollmentCommand
{
    Task AddAsync(
        Enrollment entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Enrollment entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Enrollment entity,
        CancellationToken cancellationToken);
}
