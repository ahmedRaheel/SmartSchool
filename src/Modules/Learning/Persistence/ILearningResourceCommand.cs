using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Persistence;

public interface ILearningResourceCommand
{
    Task AddAsync(
        LearningResource entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        LearningResource entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        LearningResource entity,
        CancellationToken cancellationToken);
}
