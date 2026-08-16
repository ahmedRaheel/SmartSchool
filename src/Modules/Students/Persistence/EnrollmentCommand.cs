using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence;

/// <summary>
/// Write-side persistence for Enrollment.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class EnrollmentCommand : IEnrollmentCommand
{
    public Task AddAsync(
        Enrollment entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Enrollment create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Enrollment entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Enrollment update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Enrollment entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Enrollment delete persistence has not been connected to the module DbContext.");
    }
}
