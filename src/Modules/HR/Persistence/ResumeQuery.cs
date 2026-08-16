using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Read-side persistence for Resume.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the HR module.
/// </summary>
public sealed class ResumeQuery : IResumeQuery
{
    public Task<Resume?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Resume read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Resume>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Resume paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Resume uniqueness persistence has not been connected to the module DbContext.");
    }
}
