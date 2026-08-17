using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Write-side persistence for SubjectEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class SubjectCommand : ISubjectCommand
{
    public Task AddAsync(
        SubjectEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "SubjectEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        SubjectEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "SubjectEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        SubjectEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "SubjectEntity delete persistence has not been connected to the module DbContext.");
    }
}
