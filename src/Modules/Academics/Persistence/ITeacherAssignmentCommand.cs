using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

public interface ITeacherAssignmentCommand
{
    Task AddAsync(
        TeacherAssignment entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        TeacherAssignment entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        TeacherAssignment entity,
        CancellationToken cancellationToken);
}
