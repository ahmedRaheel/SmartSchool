using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Persistence;

/// <summary>
/// Executes database reads for <see cref="GuardianEntity"/>.
/// Read operations are tenant-scoped and use no-tracking queries.
/// </summary>
public sealed class GuardianQuery(
	IApplicationDbContext dbContext,
	IDapperReadStore dapperReadStore) : IGuardianQuery
{
	public Task<GuardianEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken)
	{
		return dbContext
			.Set<GuardianEntity>()
			.AsNoTracking()
			.SingleOrDefaultAsync(
				entity => entity.TenantId == tenantId && entity.Id == id,
				cancellationToken);
	}

	public Task<PagedResult<GuardianEntity>> GetPageAsync(
		Guid tenantId,
		int page,
		int pageSize,
		CancellationToken cancellationToken)
	{
		return dapperReadStore.GetPageAsync<GuardianEntity>(
			tenantId,
			page,
			pageSize,
			[
				nameof(Entity.TenantId),
				nameof(Entity.Id),
				nameof(GuardianEntity.UserId),
				nameof(GuardianEntity.FullName),
				nameof(GuardianEntity.CnicNumber),
				nameof(GuardianEntity.Email),
				nameof(GuardianEntity.Phone)
			],
			cancellationToken);
	}

	public Task<bool> ExistsByCnicNumberAsync(
		Guid tenantId,
		string cnicNumber,
		Guid? excludingId,
		CancellationToken cancellationToken)
	{
		return dbContext
			.Set<GuardianEntity>()
			.AsNoTracking()
			.AnyAsync(
				entity =>
					entity.TenantId == tenantId && entity.CnicNumber == cnicNumber
					&& (!excludingId.HasValue || entity.Id != excludingId.Value),
				cancellationToken);
	}
}
