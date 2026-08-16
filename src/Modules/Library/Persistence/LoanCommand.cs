using SmartSchool.Modules.Library.Models;

namespace SmartSchool.Modules.Library.Persistence;

/// <summary>
/// Write-side persistence for Loan.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class LoanCommand : ILoanCommand
{
    public Task AddAsync(
        Loan entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Loan create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Loan entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Loan update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Loan entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Loan delete persistence has not been connected to the module DbContext.");
    }
}
