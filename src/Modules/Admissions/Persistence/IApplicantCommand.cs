using SmartSchool.Modules.Admissions.Models;

namespace SmartSchool.Modules.Admissions.Persistence;

public interface IApplicantCommand
{
    Task AddAsync(
        Applicant entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Applicant entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Applicant entity,
        CancellationToken cancellationToken);
}
