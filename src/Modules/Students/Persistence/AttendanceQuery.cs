using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Persistence;

/// <summary>
/// EF-backed read persistence for AttendanceEntity.
/// </summary>
public sealed class AttendanceQuery(IEfMockStore store) : IAttendanceQuery
{
	public Task<AttendanceEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<AttendanceEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<AttendanceEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<AttendanceEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<AttendanceEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
