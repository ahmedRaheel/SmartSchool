using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Persistence;

/// <summary>
/// EF-backed read persistence for StudentGuardianEntity.
/// </summary>
public sealed class StudentGuardianQuery(IEfMockStore store) : IStudentGuardianQuery
{
	public Task<StudentGuardianEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<StudentGuardianEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<StudentGuardianEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<StudentGuardianEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<StudentGuardianEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
