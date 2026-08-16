using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// Write-side persistence for StudentFee.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class StudentFeeCommand : IStudentFeeCommand
{
    public Task AddAsync(
        StudentFee entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentFee create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        StudentFee entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentFee update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        StudentFee entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentFee delete persistence has not been connected to the module DbContext.");
    }
}
