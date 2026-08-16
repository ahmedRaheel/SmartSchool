using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Write-side persistence for Program.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ProgramCommand : IProgramCommand
{
    public Task AddAsync(
        Program entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Program create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Program entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Program update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Program entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Program delete persistence has not been connected to the module DbContext.");
    }
}
