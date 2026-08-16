using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

public interface ITutorMessageCommand
{
    Task AddAsync(
        TutorMessage entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        TutorMessage entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        TutorMessage entity,
        CancellationToken cancellationToken);
}
