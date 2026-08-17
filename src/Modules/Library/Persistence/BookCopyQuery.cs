using SmartSchool.Modules.Library.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Library.Persistence;

/// <summary>
/// Read-side persistence for BookCopyEntity.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Library module.
/// </summary>
public sealed class BookCopyQuery : IBookCopyQuery
{
    public Task<BookCopyEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "BookCopyEntity read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<BookCopyEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "BookCopyEntity paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "BookCopyEntity uniqueness persistence has not been connected to the module DbContext.");
    }
}
