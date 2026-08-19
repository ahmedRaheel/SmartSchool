using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Admissions.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Admissions.Persistence;

/// <summary>
/// Executes database reads for <see cref="ApplicationEntity"/>.
/// Read operations are tenant-scoped and use no-tracking queries.
/// </summary>
public sealed class ApplicationQuery(
	IApplicationDbContext dbContext,
	IDapperReadStore dapperReadStore) : IApplicationQuery
{
	public Task<ApplicationEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken)
	{
		return dbContext
			.Set<ApplicationEntity>()
			.AsNoTracking()
			.SingleOrDefaultAsync(
				entity => entity.TenantId == tenantId && entity.Id == id,
				cancellationToken);
	}

	public Task<PagedResult<ApplicationEntity>> GetPageAsync(
		Guid tenantId,
		int page,
		int pageSize,
		CancellationToken cancellationToken)
	{
		return dapperReadStore.GetPageAsync<ApplicationEntity>(
			tenantId,
			page,
			pageSize,
			[
				nameof(Entity.TenantId),
				nameof(Entity.Id),
				nameof(ApplicationEntity.Code),
				nameof(ApplicationEntity.Name),
				nameof(ApplicationEntity.MetadataJson)
			],
			cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(
		Guid tenantId,
		string code,
		Guid? excludingId,
		CancellationToken cancellationToken)
	{
		return dbContext
			.Set<ApplicationEntity>()
			.AsNoTracking()
			.AnyAsync(
				entity =>
					entity.TenantId == tenantId
					&& EF.Property<string>(entity, "Code") == code
					&& (!excludingId.HasValue || entity.Id != excludingId.Value),
				cancellationToken);
	}
}
