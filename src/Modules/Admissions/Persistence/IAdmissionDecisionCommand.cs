using SmartSchool.Modules.Admissions.Models;

namespace SmartSchool.Modules.Admissions.Persistence;

public interface IAdmissionDecisionCommand
{
    Task AddAsync(
        AdmissionDecision entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        AdmissionDecision entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        AdmissionDecision entity,
        CancellationToken cancellationToken);
}
