using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Persistence;

/// <summary>
/// EF-backed read persistence for StudentEntity.
/// </summary>
public sealed class StudentQuery(IEfMockStore store) : IStudentQuery
{
	public Task<StudentEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<StudentEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<StudentEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<StudentEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<StudentEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
