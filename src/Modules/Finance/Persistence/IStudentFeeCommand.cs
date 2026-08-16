using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

public interface IStudentFeeCommand
{
    Task AddAsync(
        StudentFee entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        StudentFee entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        StudentFee entity,
        CancellationToken cancellationToken);
}
