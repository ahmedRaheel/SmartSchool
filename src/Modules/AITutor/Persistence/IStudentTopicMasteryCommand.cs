using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

public interface IStudentTopicMasteryCommand
{
    Task AddAsync(
        StudentTopicMastery entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        StudentTopicMastery entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        StudentTopicMastery entity,
        CancellationToken cancellationToken);
}
