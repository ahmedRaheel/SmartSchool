using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Transport.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Transport.Persistence;

/// <summary>
/// EF-backed read persistence for StudentTransportEntity.
/// </summary>
public sealed class StudentTransportQuery(IEfMockStore store) : IStudentTransportQuery
{
	public Task<StudentTransportEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<StudentTransportEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<StudentTransportEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<StudentTransportEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<StudentTransportEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
