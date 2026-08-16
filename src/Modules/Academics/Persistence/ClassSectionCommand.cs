using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Write-side persistence for ClassSection.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ClassSectionCommand : IClassSectionCommand
{
    public Task AddAsync(
        ClassSection entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ClassSection create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ClassSection entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ClassSection update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ClassSection entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ClassSection delete persistence has not been connected to the module DbContext.");
    }
}
