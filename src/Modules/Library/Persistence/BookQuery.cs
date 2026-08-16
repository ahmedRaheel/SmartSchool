using SmartSchool.Modules.Library.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Library.Persistence;

/// <summary>
/// Read-side persistence for Book.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Library module.
/// </summary>
public sealed class BookQuery : IBookQuery
{
    public Task<Book?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Book read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Book>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Book paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Book uniqueness persistence has not been connected to the module DbContext.");
    }
}
