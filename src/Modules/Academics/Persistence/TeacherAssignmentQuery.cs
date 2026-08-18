using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// EF-backed read persistence for TeacherAssignmentEntity.
/// </summary>
public sealed class TeacherAssignmentQuery(IEfMockStore store) : ITeacherAssignmentQuery
{
	public Task<TeacherAssignmentEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<TeacherAssignmentEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<TeacherAssignmentEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<TeacherAssignmentEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<TeacherAssignmentEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
