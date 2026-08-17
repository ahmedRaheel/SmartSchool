using SmartSchool.Modules.Library.Models;

namespace SmartSchool.Modules.Library.Persistence;

/// <summary>
/// Write-side persistence for LoanEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class LoanCommand : ILoanCommand
{
    public Task AddAsync(
        LoanEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LoanEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        LoanEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LoanEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        LoanEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LoanEntity delete persistence has not been connected to the module DbContext.");
    }
}
