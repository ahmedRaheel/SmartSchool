using SmartSchool.Modules.Transport.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Transport.Persistence;

/// <summary>
/// Read-side persistence for StudentTransport.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Transport module.
/// </summary>
public sealed class StudentTransportQuery : IStudentTransportQuery
{
    public Task<StudentTransport?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentTransport read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<StudentTransport>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentTransport paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentTransport uniqueness persistence has not been connected to the module DbContext.");
    }
}
