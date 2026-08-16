using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Persistence;

/// <summary>
/// Write-side persistence for LearningResource.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class LearningResourceCommand : ILearningResourceCommand
{
    public Task AddAsync(
        LearningResource entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LearningResource create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        LearningResource entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LearningResource update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        LearningResource entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LearningResource delete persistence has not been connected to the module DbContext.");
    }
}
