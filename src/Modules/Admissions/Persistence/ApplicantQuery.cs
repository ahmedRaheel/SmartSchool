using SmartSchool.Modules.Admissions.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Admissions.Persistence;

/// <summary>
/// Read-side persistence for Applicant.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Admissions module.
/// </summary>
public sealed class ApplicantQuery : IApplicantQuery
{
    public Task<Applicant?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Applicant read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Applicant>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Applicant paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Applicant uniqueness persistence has not been connected to the module DbContext.");
    }
}
