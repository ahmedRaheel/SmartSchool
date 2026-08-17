using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Persistence;

/// <summary>
/// Write-side persistence for LearningResourceEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class LearningResourceCommand : ILearningResourceCommand
{
    public Task AddAsync(
        LearningResourceEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LearningResourceEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        LearningResourceEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LearningResourceEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        LearningResourceEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LearningResourceEntity delete persistence has not been connected to the module DbContext.");
    }
}
