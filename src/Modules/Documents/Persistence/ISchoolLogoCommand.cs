using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Persistence;

public interface ISchoolLogoCommand
{
    Task AddAsync(
        SchoolLogo entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        SchoolLogo entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        SchoolLogo entity,
        CancellationToken cancellationToken);
}
