using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

public interface ICandidateCommand
{
    Task AddAsync(
        Candidate entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Candidate entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Candidate entity,
        CancellationToken cancellationToken);
}
