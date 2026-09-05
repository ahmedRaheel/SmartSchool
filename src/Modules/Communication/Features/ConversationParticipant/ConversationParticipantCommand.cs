using SmartSchool.Modules.Communication.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Features.ConversationParticipant;

/// <summary>
/// Executes database writes for <see cref="ConversationParticipantEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ConversationParticipantCommand(ICommunicationDbContext dbContext) : IConversationParticipantCommand
{
    public async Task AddAsync(
        ConversationParticipantEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.ConversationParticipants
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        ConversationParticipantEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.ConversationParticipants
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        ConversationParticipantEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.ConversationParticipants
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
