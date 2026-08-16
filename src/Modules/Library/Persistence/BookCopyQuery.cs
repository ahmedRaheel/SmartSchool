using SmartSchool.Modules.Library.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Library.Persistence;

/// <summary>
/// Read-side persistence for BookCopy.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Library module.
/// </summary>
public sealed class BookCopyQuery : IBookCopyQuery
{
    public Task<BookCopy?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "BookCopy read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<BookCopy>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "BookCopy paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "BookCopy uniqueness persistence has not been connected to the module DbContext.");
    }
}
