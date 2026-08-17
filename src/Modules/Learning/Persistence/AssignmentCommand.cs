using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Persistence;

/// <summary>
/// Write-side persistence for AssignmentEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class AssignmentCommand : IAssignmentCommand
{
    public Task AddAsync(
        AssignmentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AssignmentEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        AssignmentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AssignmentEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        AssignmentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AssignmentEntity delete persistence has not been connected to the module DbContext.");
    }
}
