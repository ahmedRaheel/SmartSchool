using SmartSchool.Modules.Communication.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Features.Message;

/// <summary>
/// Executes database writes for <see cref="MessageEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class MessageCommand(ICommunicationDbContext dbContext) : IMessageCommand
{
    public async Task AddAsync(
        MessageEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.Messages
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        MessageEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Messages
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        MessageEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Messages
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
