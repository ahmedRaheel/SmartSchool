using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Write-side persistence for TeacherAssignmentEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class TeacherAssignmentCommand : ITeacherAssignmentCommand
{
    public Task AddAsync(
        TeacherAssignmentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TeacherAssignmentEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        TeacherAssignmentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TeacherAssignmentEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        TeacherAssignmentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TeacherAssignmentEntity delete persistence has not been connected to the module DbContext.");
    }
}
