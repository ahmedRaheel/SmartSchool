using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Write-side persistence for TeacherAssignment.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class TeacherAssignmentCommand : ITeacherAssignmentCommand
{
    public Task AddAsync(
        TeacherAssignment entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TeacherAssignment create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        TeacherAssignment entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TeacherAssignment update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        TeacherAssignment entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TeacherAssignment delete persistence has not been connected to the module DbContext.");
    }
}
