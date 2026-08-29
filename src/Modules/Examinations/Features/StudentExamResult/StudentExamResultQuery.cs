using Dapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Examinations.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Examinations.Features.StudentExamResult;

/// <summary>
/// Executes database reads for <see cref="StudentExamResultEntity"/>.
/// Read operations are tenant-scoped and use no-tracking queries.
/// </summary>
public sealed class StudentExamResultQuery(
	IApplicationDbContext dbContext,
	IDbConnectionFactory connectionFactory) : IStudentExamResultQuery
{
	public Task<StudentExamResultEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken)
	{
		return dbContext
			.Set<StudentExamResultEntity>()
			.AsNoTracking()
			.SingleOrDefaultAsync(
				entity => entity.TenantId == tenantId && entity.StudentExamResultId == id,
				cancellationToken);
	}

	public async Task<PagedResult<StudentExamResultEntity>> GetPageAsync(
		Guid tenantId,
		int page,
		int pageSize,
		CancellationToken cancellationToken)
	{
		const string countSql = """
			SELECT COUNT(*)
			FROM exam.student_exam_result
			WHERE tenant_id = @TenantId
			  AND is_active = TRUE;
			""";

		const string pageSql = """
			SELECT
				tenant_id AS "TenantId",
				student_exam_result_id AS "Id"
			FROM exam.student_exam_result
			WHERE tenant_id = @TenantId
			  AND is_active = TRUE
			ORDER BY student_exam_result_id
			LIMIT @PageSize OFFSET @Offset;
			""";

		await using var connection =
			await connectionFactory.OpenConnectionAsync(cancellationToken);

		var parameters = new
		{
			TenantId = tenantId,
			PageSize = pageSize,
			Offset = (page - 1) * pageSize
		};

		var totalCount = await connection.ExecuteScalarAsync<long>(
			new CommandDefinition(
				countSql,
				parameters,
				cancellationToken: cancellationToken));

		var items = (await connection.QueryAsync<StudentExamResultEntity>(
			new CommandDefinition(
				pageSql,
				parameters,
				cancellationToken: cancellationToken)))
			.AsList();

		return new PagedResult<StudentExamResultEntity>(
			items,
			page,
			pageSize,
			totalCount);
	}

	public async Task<IReadOnlyCollection<StudentExamResultEntity>> GetByStudentIdAsync(
		Guid tenantId,
		Guid studentId,
		int limit,
		CancellationToken cancellationToken)
	{
		var pageSize = Math.Clamp(limit, 1, 100);

		return await dbContext
			.Set<StudentExamResultEntity>()
			.AsNoTracking()
			.Where(entity => entity.TenantId == tenantId && entity.StudentId == studentId)
			.OrderByDescending(entity => entity.CreatedAt)
			.Take(pageSize)
			.ToArrayAsync(cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(
		Guid tenantId,
		string code,
		Guid? excludingId,
		CancellationToken cancellationToken)
	{
		return dbContext
			.Set<StudentExamResultEntity>()
			.AsNoTracking()
			.AnyAsync(
				entity =>
					entity.TenantId == tenantId
					&& EF.Property<string>(entity, "Code") == code
					&& (!excludingId.HasValue || (excludingId.HasValue && entity.StudentExamResultId != excludingId.Value)),
				cancellationToken);
	}
}
