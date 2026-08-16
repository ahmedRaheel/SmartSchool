using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence;

public interface IStudentGuardianCommand
{
    Task AddAsync(
        StudentGuardian entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        StudentGuardian entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        StudentGuardian entity,
        CancellationToken cancellationToken);
}
