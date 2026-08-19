using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Persistence;

/// <summary>
/// Executes database reads for <see cref="StudentEntity"/>.
/// Read operations are tenant-scoped and use no-tracking queries.
/// </summary>
public sealed class StudentQuery(
	IApplicationDbContext dbContext,
	IDapperReadStore dapperReadStore) : IStudentQuery
{
	public Task<StudentEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken)
	{
		return dbContext
			.Set<StudentEntity>()
			.AsNoTracking()
			.SingleOrDefaultAsync(
				entity => entity.TenantId == tenantId && entity.Id == id,
				cancellationToken);
	}

	public Task<PagedResult<StudentEntity>> GetPageAsync(
		Guid tenantId,
		int page,
		int pageSize,
		CancellationToken cancellationToken)
	{
		return dapperReadStore.GetPageAsync<StudentEntity>(
			tenantId,
			page,
			pageSize,
			[
				nameof(Entity.TenantId),
				nameof(Entity.Id),
				nameof(StudentEntity.StudentNumber),
				nameof(StudentEntity.FirstName),
				nameof(StudentEntity.LastName),
				nameof(StudentEntity.DateOfBirth),
				nameof(StudentEntity.Gender),
				nameof(StudentEntity.AdmissionDate),
				nameof(StudentEntity.Status)
			],
			cancellationToken);
	}

	public Task<bool> ExistsByStudentNumberAsync(
		Guid tenantId,
		string studentNumber,
		Guid? excludingId,
		CancellationToken cancellationToken)
	{
		return dbContext
			.Set<StudentEntity>()
			.AsNoTracking()
			.AnyAsync(
				entity =>
					entity.TenantId == tenantId && entity.StudentNumber == studentNumber
					&& (!excludingId.HasValue || entity.Id != excludingId.Value),
				cancellationToken);
	}
}
