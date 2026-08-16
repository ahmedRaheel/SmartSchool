using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence;

public interface IMessageCommand
{
    Task AddAsync(
        Message entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Message entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Message entity,
        CancellationToken cancellationToken);
}
