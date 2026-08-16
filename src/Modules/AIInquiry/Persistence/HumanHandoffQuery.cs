using SmartSchool.Modules.AIInquiry.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIInquiry.Persistence;

/// <summary>
/// Read-side persistence for HumanHandoff.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the AIInquiry module.
/// </summary>
public sealed class HumanHandoffQuery : IHumanHandoffQuery
{
    public Task<HumanHandoff?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "HumanHandoff read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<HumanHandoff>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "HumanHandoff paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "HumanHandoff uniqueness persistence has not been connected to the module DbContext.");
    }
}
