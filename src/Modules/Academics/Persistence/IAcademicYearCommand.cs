using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

public interface IAcademicYearCommand
{
    Task AddAsync(
        AcademicYear entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        AcademicYear entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        AcademicYear entity,
        CancellationToken cancellationToken);
}
