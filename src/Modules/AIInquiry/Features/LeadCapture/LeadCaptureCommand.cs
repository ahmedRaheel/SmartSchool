using SmartSchool.Modules.AIInquiry.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Features.LeadCapture;

/// <summary>
/// Executes database writes for <see cref="LeadCaptureEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class LeadCaptureCommand(IAIInquiryDbContext dbContext) : ILeadCaptureCommand
{
    public async Task AddAsync(
        LeadCaptureEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.LeadCaptures
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        LeadCaptureEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.LeadCaptures
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        LeadCaptureEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.LeadCaptures
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
