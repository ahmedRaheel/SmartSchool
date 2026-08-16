using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Persistence;

public interface IStudentTransportCommand
{
    Task AddAsync(
        StudentTransport entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        StudentTransport entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        StudentTransport entity,
        CancellationToken cancellationToken);
}
