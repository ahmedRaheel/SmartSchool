using SmartSchool.Modules.AIParent.Models;

namespace SmartSchool.Modules.AIParent.Persistence;

public interface IParentMessageCommand
{
    Task AddAsync(
        ParentMessage entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        ParentMessage entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        ParentMessage entity,
        CancellationToken cancellationToken);
}
