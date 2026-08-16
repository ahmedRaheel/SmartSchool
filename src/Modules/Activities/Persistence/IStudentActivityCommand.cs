using SmartSchool.Modules.Activities.Models;

namespace SmartSchool.Modules.Activities.Persistence;

public interface IStudentActivityCommand
{
    Task AddAsync(
        StudentActivity entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        StudentActivity entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        StudentActivity entity,
        CancellationToken cancellationToken);
}
