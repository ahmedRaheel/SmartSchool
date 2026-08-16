using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Read-side persistence for StudentPerformancePrediction.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the AIPrediction module.
/// </summary>
public sealed class StudentPerformancePredictionQuery : IStudentPerformancePredictionQuery
{
    public Task<StudentPerformancePrediction?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentPerformancePrediction read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<StudentPerformancePrediction>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentPerformancePrediction paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentPerformancePrediction uniqueness persistence has not been connected to the module DbContext.");
    }
}
