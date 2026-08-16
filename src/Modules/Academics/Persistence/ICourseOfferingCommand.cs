using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

public interface ICourseOfferingCommand
{
    Task AddAsync(
        CourseOffering entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        CourseOffering entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        CourseOffering entity,
        CancellationToken cancellationToken);
}
