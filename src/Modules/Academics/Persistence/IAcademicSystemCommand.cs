using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

public interface IAcademicSystemCommand
{
    Task AddAsync(
        AcademicSystem entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        AcademicSystem entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        AcademicSystem entity,
        CancellationToken cancellationToken);
}
