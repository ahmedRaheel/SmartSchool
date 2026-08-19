using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Examinations.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Examinations.Persistence;

/// <summary>
/// Executes database reads for <see cref="ExamSubjectEntity"/>.
/// Read operations are tenant-scoped and use no-tracking queries.
/// </summary>
public sealed class ExamSubjectQuery(
	IApplicationDbContext dbContext,
	IDapperReadStore dapperReadStore) : IExamSubjectQuery
{
	public Task<ExamSubjectEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken)
	{
		return dbContext
			.Set<ExamSubjectEntity>()
			.AsNoTracking()
			.SingleOrDefaultAsync(
				entity => entity.TenantId == tenantId && entity.Id == id,
				cancellationToken);
	}

	public Task<PagedResult<ExamSubjectEntity>> GetPageAsync(
		Guid tenantId,
		int page,
		int pageSize,
		CancellationToken cancellationToken)
	{
		return dapperReadStore.GetPageAsync<ExamSubjectEntity>(
			tenantId,
			page,
			pageSize,
			[
				nameof(Entity.TenantId),
				nameof(Entity.Id),
				nameof(ExamSubjectEntity.Code),
				nameof(ExamSubjectEntity.Name),
				nameof(ExamSubjectEntity.MetadataJson)
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
			.Set<ExamSubjectEntity>()
			.AsNoTracking()
			.AnyAsync(
				entity =>
					entity.TenantId == tenantId
					&& EF.Property<string>(entity, "Code") == code
					&& (!excludingId.HasValue || entity.Id != excludingId.Value),
				cancellationToken);
	}
}
