using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Write-side persistence for Subject.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class SubjectCommand : ISubjectCommand
{
    public Task AddAsync(
        Subject entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Subject create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Subject entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Subject update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Subject entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Subject delete persistence has not been connected to the module DbContext.");
    }
}
