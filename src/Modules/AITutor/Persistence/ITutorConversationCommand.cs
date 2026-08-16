using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

public interface ITutorConversationCommand
{
    Task AddAsync(
        TutorConversation entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        TutorConversation entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        TutorConversation entity,
        CancellationToken cancellationToken);
}
