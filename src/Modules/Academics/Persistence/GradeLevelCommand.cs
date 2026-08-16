using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Write-side persistence for GradeLevel.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class GradeLevelCommand : IGradeLevelCommand
{
    public Task AddAsync(
        GradeLevel entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GradeLevel create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        GradeLevel entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GradeLevel update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        GradeLevel entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GradeLevel delete persistence has not been connected to the module DbContext.");
    }
}
