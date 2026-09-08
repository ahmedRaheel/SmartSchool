using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Features.Enrollment;

public static class GetEnrollmentByClassSectionId
{
    public sealed record Response(
    Guid TenantId,
    Guid Id,
    Guid StudentId,
    Guid AcademicYearId,
    Guid ClassSectionId,
    DateOnly EnrollmentDate,
    string Status,
    string? AcademicYearCode,
    string? AcademicYearName,
    string? ClassSectionCode,
    string? ClassSectionName);

    public sealed record Query(Guid TenantId, Guid ParentId)
        : IRequest<Result<IReadOnlyCollection<Response>>>;

    public interface IGetEnrollmentByClassSectionIdQuery
    {
        Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid parentId,
            CancellationToken cancellationToken);
    }

    internal sealed class GetEnrollmentByClassSectionIdQuery(IDbConnectionFactory connectionFactory)
        : IGetEnrollmentByClassSectionIdQuery
    {
        public async Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid parentId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT entity.tenant_id AS "TenantId", entity.student_enrollment_id AS "Id", entity.student_id AS "StudentId", entity.academic_year_id AS "AcademicYearId", entity.class_section_id AS "ClassSectionId", entity.enrollment_date AS "EnrollmentDate", entity.status AS "Status",
                        p1.code AS "AcademicYearCode",
                        p1.name AS "AcademicYearName",
                        p2.code AS "ClassSectionCode",
                        p2.name AS "ClassSectionName"
                FROM student.student_enrollment AS entity
                    LEFT JOIN academic.academic_year AS p1
                        ON p1.academic_year_id = entity.academic_year_id
                    LEFT JOIN academic.class_section AS p2
                        ON p2.class_section_id = entity.class_section_id
                WHERE entity.tenant_id = @TenantId
                  AND entity.class_section_id = @ParentId
                  AND entity.is_active = TRUE;
                """;

            await using var connection =
                await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

            var items = await connection.QueryAsync<Response>(
                new CommandDefinition(
                    sql,
                    new { TenantId = tenantId, ParentId = parentId },
                    cancellationToken: cancellationToken)).ConfigureAwait(false);

            return items.AsList();
        }
    }

    public sealed class Handler(IGetEnrollmentByClassSectionIdQuery query)
        : IRequestHandler<Query, Result<IReadOnlyCollection<Response>>>
    {
        public async Task<Result<IReadOnlyCollection<Response>>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var items = await query.GetAsync(
                request.TenantId,
                request.ParentId,
                cancellationToken).ConfigureAwait(false);

            return Result<IReadOnlyCollection<Response>>.Success(items);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/students/student-enrollment/by-class-section/{parentId:guid}",
                async (Guid parentId, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Query, Result<IReadOnlyCollection<Response>>>(
                        new Query(tenantId, parentId),
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetEnrollmentByClassSectionId")
            .WithTags("Students")
            .RequireAuthorization();

        return endpoints;
    }
}
