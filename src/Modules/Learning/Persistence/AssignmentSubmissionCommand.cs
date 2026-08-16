using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Persistence;

/// <summary>
/// Write-side persistence for AssignmentSubmission.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class AssignmentSubmissionCommand : IAssignmentSubmissionCommand
{
    public Task AddAsync(
        AssignmentSubmission entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AssignmentSubmission create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        AssignmentSubmission entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AssignmentSubmission update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        AssignmentSubmission entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AssignmentSubmission delete persistence has not been connected to the module DbContext.");
    }
}
