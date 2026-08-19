using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Executes database reads for <see cref="EmployeeEntity"/>.
/// Read operations are tenant-scoped and use no-tracking queries.
/// </summary>
public sealed class EmployeeQuery(
	IApplicationDbContext dbContext,
	IDapperReadStore dapperReadStore) : IEmployeeQuery
{
	public Task<EmployeeEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken)
	{
		return dbContext
			.Set<EmployeeEntity>()
			.AsNoTracking()
			.SingleOrDefaultAsync(
				entity => entity.TenantId == tenantId && entity.Id == id,
				cancellationToken);
	}

	public Task<PagedResult<EmployeeEntity>> GetPageAsync(
		Guid tenantId,
		int page,
		int pageSize,
		CancellationToken cancellationToken)
	{
		return dapperReadStore.GetPageAsync<EmployeeEntity>(
			tenantId,
			page,
			pageSize,
			[
				nameof(Entity.TenantId),
				nameof(Entity.Id),
				nameof(EmployeeEntity.EmployeeNumber),
				nameof(EmployeeEntity.FirstName),
				nameof(EmployeeEntity.LastName),
				nameof(EmployeeEntity.CnicNumber),
				nameof(EmployeeEntity.Email),
				nameof(EmployeeEntity.Phone),
				nameof(EmployeeEntity.HireDate),
				nameof(EmployeeEntity.EmploymentTypeCode),
				nameof(EmployeeEntity.Status)
			],
			cancellationToken);
	}

	public Task<bool> ExistsByEmployeeNumberAsync(
		Guid tenantId,
		string employeeNumber,
		Guid? excludingId,
		CancellationToken cancellationToken)
	{
		return dbContext
			.Set<EmployeeEntity>()
			.AsNoTracking()
			.AnyAsync(
				entity =>
					entity.TenantId == tenantId && entity.EmployeeNumber == employeeNumber
					&& (!excludingId.HasValue || entity.Id != excludingId.Value),
				cancellationToken);
	}
}
