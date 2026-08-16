using SmartSchool.Modules.Library.Models;

namespace SmartSchool.Modules.Library.Persistence;

public interface ILoanCommand
{
    Task AddAsync(
        Loan entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Loan entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Loan entity,
        CancellationToken cancellationToken);
}
