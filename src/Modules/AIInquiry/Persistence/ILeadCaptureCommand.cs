using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Persistence;

public interface ILeadCaptureCommand
{
    Task AddAsync(
        LeadCapture entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        LeadCapture entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        LeadCapture entity,
        CancellationToken cancellationToken);
}
