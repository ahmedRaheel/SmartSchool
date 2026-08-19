using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Executes database reads for <see cref="ClassSectionEntity"/>.
/// Read operations are tenant-scoped and use no-tracking queries.
/// </summary>
public sealed class ClassSectionQuery(
	IApplicationDbContext dbContext,
	IDapperReadStore dapperReadStore) : IClassSectionQuery
{
	public Task<ClassSectionEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken)
	{
		return dbContext
			.Set<ClassSectionEntity>()
			.AsNoTracking()
			.SingleOrDefaultAsync(
				entity => entity.TenantId == tenantId && entity.Id == id,
				cancellationToken);
	}

	public Task<PagedResult<ClassSectionEntity>> GetPageAsync(
		Guid tenantId,
		int page,
		int pageSize,
		CancellationToken cancellationToken)
	{
		return dapperReadStore.GetPageAsync<ClassSectionEntity>(
			tenantId,
			page,
			pageSize,
			[
				nameof(Entity.TenantId),
				nameof(Entity.Id),
				nameof(ClassSectionEntity.Code),
				nameof(ClassSectionEntity.Name),
				nameof(ClassSectionEntity.MetadataJson)
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
			.Set<ClassSectionEntity>()
			.AsNoTracking()
			.AnyAsync(
				entity =>
					entity.TenantId == tenantId
					&& EF.Property<string>(entity, "Code") == code
					&& (!excludingId.HasValue || entity.Id != excludingId.Value),
				cancellationToken);
	}
}
