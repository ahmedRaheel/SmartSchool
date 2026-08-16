using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Persistence;

/// <summary>
/// Write-side persistence for StudentTransport.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class StudentTransportCommand : IStudentTransportCommand
{
    public Task AddAsync(
        StudentTransport entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentTransport create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        StudentTransport entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentTransport update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        StudentTransport entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentTransport delete persistence has not been connected to the module DbContext.");
    }
}
