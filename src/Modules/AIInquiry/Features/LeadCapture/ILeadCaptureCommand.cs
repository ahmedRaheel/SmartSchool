using System.Threading.Tasks;
using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Features.LeadCapture;

/// <summary>
/// Defines command persistence operations for LeadCaptureEntity.
/// </summary>
public interface ILeadCaptureCommand
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task AddAsync(
        LeadCaptureEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task UpdateAsync(
        LeadCaptureEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task DeleteAsync(
        LeadCaptureEntity entity,
        CancellationToken cancellationToken);
}
