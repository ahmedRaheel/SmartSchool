using SmartSchool.Modules.Students.Persistence;
using Dapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Features.Enrollment;

/// <summary>
/// Executes database reads for <see cref="EnrollmentEntity"/>.
/// Read operations are tenant-scoped and use no-tracking queries.
/// </summary>
public sealed class EnrollmentQuery(
    IStudentsDbContext dbContext,
    IDbConnectionFactory connectionFactory) : IEnrollmentQuery
{
    public Task<EnrollmentEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        return dbContext.Enrollments
            .AsNoTracking()
            .SingleOrDefaultAsync(
                entity => entity.TenantId == tenantId && entity.StudentEnrollmentId == id,
                cancellationToken);
    }

    public async Task<PagedResult<EnrollmentEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        const string countSql = """
            SELECT COUNT(*)
            FROM student.student_enrollment
            WHERE tenant_id = @TenantId
              AND is_active = TRUE;
            """;

        const string pageSql = """
            SELECT
                tenant_id AS "TenantId",
                student_enrollment_id AS "Id"
            FROM student.student_enrollment
            WHERE tenant_id = @TenantId
              AND is_active = TRUE
            ORDER BY student_enrollment_id
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

        var items = (await connection.QueryAsync<EnrollmentEntity>(
            new CommandDefinition(
                pageSql,
                parameters,
                cancellationToken: cancellationToken)))
            .AsList();

        return new PagedResult<EnrollmentEntity>(
            items,
            page,
            pageSize,
            totalCount);
    }

    public Task<bool> ExistsForAcademicYearAsync(
        Guid tenantId,
        Guid studentId,
        Guid academicYearId,
        CancellationToken cancellationToken)
    {
        return dbContext.Enrollments.AsNoTracking().AnyAsync(
            entity => entity.TenantId == tenantId
                && entity.StudentId == studentId
                && entity.AcademicYearId == academicYearId,
            cancellationToken);
    }
}
