using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence;

/// <summary>
/// Write-side persistence for EnrollmentEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class EnrollmentCommand : IEnrollmentCommand
{
    public Task AddAsync(
        EnrollmentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "EnrollmentEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        EnrollmentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "EnrollmentEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        EnrollmentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "EnrollmentEntity delete persistence has not been connected to the module DbContext.");
    }
}
