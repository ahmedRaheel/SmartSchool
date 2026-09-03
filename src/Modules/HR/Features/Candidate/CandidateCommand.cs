using SmartSchool.Modules.HR.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Features.Candidate;

/// <summary>
/// Executes database writes for <see cref="CandidateEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class CandidateCommand(IHRDbContext dbContext) : ICandidateCommand
{
    public async Task AddAsync(
        CandidateEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.Candidates
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        CandidateEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Candidates
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        CandidateEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Candidates
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
