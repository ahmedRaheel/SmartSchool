using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// Write-side persistence for TutorConversation.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class TutorConversationCommand : ITutorConversationCommand
{
    public Task AddAsync(
        TutorConversation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TutorConversation create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        TutorConversation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TutorConversation update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        TutorConversation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TutorConversation delete persistence has not been connected to the module DbContext.");
    }
}
