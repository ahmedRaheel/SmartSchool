using SmartSchool.Modules.AIParent.Models;

namespace SmartSchool.Modules.AIParent.Persistence;

/// <summary>
/// Write-side persistence for ParentMessageEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ParentMessageCommand : IParentMessageCommand
{
    public Task AddAsync(
        ParentMessageEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentMessageEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ParentMessageEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentMessageEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ParentMessageEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentMessageEntity delete persistence has not been connected to the module DbContext.");
    }
}
