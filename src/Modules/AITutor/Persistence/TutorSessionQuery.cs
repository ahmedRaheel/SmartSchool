using SmartSchool.Modules.AITutor.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// Read-side persistence for TutorSession.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the AITutor module.
/// </summary>
public sealed class TutorSessionQuery : ITutorSessionQuery
{
    public Task<TutorSession?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TutorSession read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<TutorSession>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TutorSession paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TutorSession uniqueness persistence has not been connected to the module DbContext.");
    }
}
