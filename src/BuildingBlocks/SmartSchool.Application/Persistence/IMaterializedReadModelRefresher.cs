using System.Threading.Tasks;
namespace SmartSchool.Application.Persistence;

/// <summary>
/// Refreshes denormalized read tables after transactional data changes.
/// Implementations should be idempotent and safe to retry.
/// </summary>
public interface IMaterializedReadModelRefresher
{
    Task RefreshStudentAsync(
        Guid tenantId,
        Guid studentId,
        CancellationToken cancellationToken);

    Task RefreshTeacherAsync(
        Guid tenantId,
        Guid teacherId,
        CancellationToken cancellationToken);

    Task RefreshDriverAsync(
        Guid tenantId,
        Guid driverId,
        CancellationToken cancellationToken);
}
