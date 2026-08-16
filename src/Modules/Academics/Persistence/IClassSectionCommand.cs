using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

public interface IClassSectionCommand
{
    Task AddAsync(
        ClassSection entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        ClassSection entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        ClassSection entity,
        CancellationToken cancellationToken);
}
