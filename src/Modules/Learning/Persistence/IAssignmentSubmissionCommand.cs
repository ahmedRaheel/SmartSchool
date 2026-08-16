using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Persistence;

public interface IAssignmentSubmissionCommand
{
    Task AddAsync(
        AssignmentSubmission entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        AssignmentSubmission entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        AssignmentSubmission entity,
        CancellationToken cancellationToken);
}
