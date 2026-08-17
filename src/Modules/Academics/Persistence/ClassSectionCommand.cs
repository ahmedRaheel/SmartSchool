using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Write-side persistence for ClassSectionEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ClassSectionCommand : IClassSectionCommand
{
    public Task AddAsync(
        ClassSectionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ClassSectionEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ClassSectionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ClassSectionEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ClassSectionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ClassSectionEntity delete persistence has not been connected to the module DbContext.");
    }
}
