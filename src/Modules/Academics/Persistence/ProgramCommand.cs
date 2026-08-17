using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Write-side persistence for ProgramEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ProgramCommand : IProgramCommand
{
    public Task AddAsync(
        ProgramEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ProgramEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ProgramEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ProgramEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ProgramEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ProgramEntity delete persistence has not been connected to the module DbContext.");
    }
}
