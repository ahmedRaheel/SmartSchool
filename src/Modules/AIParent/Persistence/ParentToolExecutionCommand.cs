using SmartSchool.Modules.AIParent.Models;

namespace SmartSchool.Modules.AIParent.Persistence;

/// <summary>
/// Write-side persistence for ParentToolExecution.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ParentToolExecutionCommand : IParentToolExecutionCommand
{
    public Task AddAsync(
        ParentToolExecution entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentToolExecution create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ParentToolExecution entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentToolExecution update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ParentToolExecution entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentToolExecution delete persistence has not been connected to the module DbContext.");
    }
}
