using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Features.ClassPerformanceInsight;

public static class GetClassPerformanceInsightByTeacherEmployeeId
{
    public sealed record Response(
    Guid TenantId,
    Guid Id,
    string Code,
    string Name,
    string? MetadataJson,
    Guid AcademicYearId,
    string? AcademicYearCode,
    string? AcademicYearName,
    Guid ClassSectionId,
    string? ClassSectionCode,
    string? ClassSectionName,
    Guid CourseOfferingId,
    string? CourseOfferingCode,
    string? CourseOfferingName,
    Guid? TermId,
    string? TermCode,
    string? TermName);

    public sealed record Query(Guid TenantId, Guid ParentId)
        : IRequest<Result<IReadOnlyCollection<Response>>>;

    public interface IGetClassPerformanceInsightByTeacherEmployeeIdQuery
    {
        Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid parentId,
            CancellationToken cancellationToken);
    }

    internal sealed class GetClassPerformanceInsightByTeacherEmployeeIdQuery(IDbConnectionFactory connectionFactory)
        : IGetClassPerformanceInsightByTeacherEmployeeIdQuery
    {
        public async Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid parentId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                    SELECT
                        entity.tenant_id AS "TenantId",
                        entity.class_performance_insight_id AS "Id",
                        entity.code AS "Code",
                        entity.name AS "Name",
                        entity.metadata_json AS "MetadataJson",
                        p1.academic_year_id AS "AcademicYearId",
                        p1.code AS "AcademicYearCode",
                        p1.name AS "AcademicYearName",
                        p2.class_section_id AS "ClassSectionId",
                        p2.code AS "ClassSectionCode",
                        p2.name AS "ClassSectionName",
                        p3.course_offering_id AS "CourseOfferingId",
                        p3.code AS "CourseOfferingCode",
                        p3.name AS "CourseOfferingName",
                        p4.term_id AS "TermId",
                        p4.code AS "TermCode",
                        p4.name AS "TermName"
                    FROM ai.class_performance_insight AS entity
                    LEFT JOIN academic.academic_year AS p1
                        ON p1.academic_year_id = entity.academic_year_id
                    LEFT JOIN academic.class_section AS p2
                        ON p2.class_section_id = entity.class_section_id
                    LEFT JOIN academic.course_offering AS p3
                        ON p3.course_offering_id = entity.course_offering_id
                    LEFT JOIN academic.term AS p4
                        ON p4.term_id = entity.term_id
                    WHERE entity.tenant_id = @TenantId
                      AND entity.teacher_employee_id = @ParentId
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

    public sealed class Handler(IGetClassPerformanceInsightByTeacherEmployeeIdQuery query)
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
                "/api/aiprediction/class-performance-insight/by-teacher-employee/{parentId:guid}",
                async (Guid parentId, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Query, Result<IReadOnlyCollection<Response>>>(
                        new Query(tenantId, parentId),
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetClassPerformanceInsightByTeacherEmployeeId")
            .WithTags("AIPrediction")
            .RequireAuthorization();

        return endpoints;
    }
}
