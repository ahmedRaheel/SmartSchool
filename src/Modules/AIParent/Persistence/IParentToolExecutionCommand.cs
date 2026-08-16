using SmartSchool.Modules.AIParent.Models;

namespace SmartSchool.Modules.AIParent.Persistence;

public interface IParentToolExecutionCommand
{
    Task AddAsync(
        ParentToolExecution entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        ParentToolExecution entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        ParentToolExecution entity,
        CancellationToken cancellationToken);
}
