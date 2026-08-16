using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Persistence;

/// <summary>
/// Write-side persistence for Assignment.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class AssignmentCommand : IAssignmentCommand
{
    public Task AddAsync(
        Assignment entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Assignment create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Assignment entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Assignment update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Assignment entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Assignment delete persistence has not been connected to the module DbContext.");
    }
}
