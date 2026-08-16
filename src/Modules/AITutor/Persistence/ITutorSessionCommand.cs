using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

public interface ITutorSessionCommand
{
    Task AddAsync(
        TutorSession entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        TutorSession entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        TutorSession entity,
        CancellationToken cancellationToken);
}
