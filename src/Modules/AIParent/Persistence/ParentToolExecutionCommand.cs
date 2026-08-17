using SmartSchool.Modules.AIParent.Models;

namespace SmartSchool.Modules.AIParent.Persistence;

/// <summary>
/// Write-side persistence for ParentToolExecutionEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ParentToolExecutionCommand : IParentToolExecutionCommand
{
    public Task AddAsync(
        ParentToolExecutionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentToolExecutionEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ParentToolExecutionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentToolExecutionEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ParentToolExecutionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentToolExecutionEntity delete persistence has not been connected to the module DbContext.");
    }
}
