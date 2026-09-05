using System.Threading.Tasks;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AITutor.Features.GeneratedQuiz;

/// <summary>
/// Defines query persistence operations for GeneratedQuizEntity.
/// </summary>
public interface IGeneratedQuizQuery
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<GeneratedQuizEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<PagedResult<GeneratedQuizEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken);
}
