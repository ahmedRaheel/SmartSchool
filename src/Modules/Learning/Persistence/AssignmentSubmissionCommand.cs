using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Persistence;

/// <summary>
/// Write-side persistence for AssignmentSubmissionEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class AssignmentSubmissionCommand : IAssignmentSubmissionCommand
{
    public Task AddAsync(
        AssignmentSubmissionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AssignmentSubmissionEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        AssignmentSubmissionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AssignmentSubmissionEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        AssignmentSubmissionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AssignmentSubmissionEntity delete persistence has not been connected to the module DbContext.");
    }
}
