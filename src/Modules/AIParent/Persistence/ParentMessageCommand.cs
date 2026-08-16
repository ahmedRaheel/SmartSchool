using SmartSchool.Modules.AIParent.Models;

namespace SmartSchool.Modules.AIParent.Persistence;

/// <summary>
/// Write-side persistence for ParentMessage.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ParentMessageCommand : IParentMessageCommand
{
    public Task AddAsync(
        ParentMessage entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentMessage create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ParentMessage entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentMessage update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ParentMessage entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentMessage delete persistence has not been connected to the module DbContext.");
    }
}
