using SmartSchool.Modules.AITutor.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// Read-side persistence for StudentTopicMastery.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the AITutor module.
/// </summary>
public sealed class StudentTopicMasteryQuery : IStudentTopicMasteryQuery
{
    public Task<StudentTopicMastery?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentTopicMastery read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<StudentTopicMastery>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentTopicMastery paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentTopicMastery uniqueness persistence has not been connected to the module DbContext.");
    }
}
