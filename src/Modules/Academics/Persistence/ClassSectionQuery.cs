using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// EF-backed read persistence for ClassSectionEntity.
/// </summary>
public sealed class ClassSectionQuery(IEfMockStore store) : IClassSectionQuery
{
	public Task<ClassSectionEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<ClassSectionEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<ClassSectionEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<ClassSectionEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<ClassSectionEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
