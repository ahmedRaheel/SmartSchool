using SmartSchool.Modules.Admissions.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Admissions.Persistence;

/// <summary>
/// Read-side persistence for AdmissionDecision.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Admissions module.
/// </summary>
public sealed class AdmissionDecisionQuery : IAdmissionDecisionQuery
{
    public Task<AdmissionDecision?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AdmissionDecision read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<AdmissionDecision>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AdmissionDecision paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AdmissionDecision uniqueness persistence has not been connected to the module DbContext.");
    }
}
