using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Read-side persistence for Interview.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the HR module.
/// </summary>
public sealed class InterviewQuery : IInterviewQuery
{
    public Task<Interview?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Interview read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Interview>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Interview paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Interview uniqueness persistence has not been connected to the module DbContext.");
    }
}
