using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// Write-side persistence for StudentFeeEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class StudentFeeCommand : IStudentFeeCommand
{
    public Task AddAsync(
        StudentFeeEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentFeeEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        StudentFeeEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentFeeEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        StudentFeeEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentFeeEntity delete persistence has not been connected to the module DbContext.");
    }
}
