using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

public interface IStudentPerformancePredictionCommand
{
    Task AddAsync(
        StudentPerformancePrediction entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        StudentPerformancePrediction entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        StudentPerformancePrediction entity,
        CancellationToken cancellationToken);
}
