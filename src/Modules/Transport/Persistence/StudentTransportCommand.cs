using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Persistence;

/// <summary>
/// Write-side persistence for StudentTransportEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class StudentTransportCommand : IStudentTransportCommand
{
    public Task AddAsync(
        StudentTransportEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentTransportEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        StudentTransportEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentTransportEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        StudentTransportEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentTransportEntity delete persistence has not been connected to the module DbContext.");
    }
}
