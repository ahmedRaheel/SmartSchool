using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Examinations.Features.Exam;

public static class GetExamByAcademicSystemId
{
    public sealed record Response(
    Guid TenantId,
    Guid Id,
    string Code,
    string Name,
    string? MetadataJson,
    Guid AcademicSystemId,
    string? AcademicSystemCode,
    string? AcademicSystemName,
    Guid AcademicYearId,
    string? AcademicYearCode,
    string? AcademicYearName,
    Guid CampusId,
    string? CampusCode,
    string? CampusName,
    Guid? TermId,
    string? TermCode,
    string? TermName);

    public sealed record Query(Guid TenantId, Guid ParentId)
        : IRequest<Result<IReadOnlyCollection<Response>>>;

    public interface IGetExamByAcademicSystemIdQuery
    {
        Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid parentId,
            CancellationToken cancellationToken);
    }

    internal sealed class GetExamByAcademicSystemIdQuery(IDbConnectionFactory connectionFactory)
        : IGetExamByAcademicSystemIdQuery
    {
        public async Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid parentId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                    SELECT
                        entity.tenant_id AS "TenantId",
                        entity.exam_id AS "Id",
                        entity.code AS "Code",
                        entity.name AS "Name",
                        entity.metadata_json AS "MetadataJson",
                        p1.academic_system_id AS "AcademicSystemId",
                        p1.code AS "AcademicSystemCode",
                        p1.name AS "AcademicSystemName",
                        p2.academic_year_id AS "AcademicYearId",
                        p2.code AS "AcademicYearCode",
                        p2.name AS "AcademicYearName",
                        p3.campus_id AS "CampusId",
                        p3.code AS "CampusCode",
                        p3.name AS "CampusName",
                        p4.term_id AS "TermId",
                        p4.code AS "TermCode",
                        p4.name AS "TermName"
                    FROM exam.exam AS entity
                    LEFT JOIN academic.academic_system AS p1
                        ON p1.academic_system_id = entity.academic_system_id
                    LEFT JOIN academic.academic_year AS p2
                        ON p2.academic_year_id = entity.academic_year_id
                    LEFT JOIN org.campus AS p3
                        ON p3.campus_id = entity.campus_id
                    LEFT JOIN academic.term AS p4
                        ON p4.term_id = entity.term_id
                    WHERE entity.tenant_id = @TenantId
                      AND entity.academic_system_id = @ParentId
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

    public sealed class Handler(IGetExamByAcademicSystemIdQuery query)
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
                "/api/examinations/exam/by-academic-system/{parentId:guid}",
                async (Guid parentId, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Query, Result<IReadOnlyCollection<Response>>>(
                        new Query(tenantId, parentId),
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetExamByAcademicSystemId")
            .WithTags("Examinations")
            .RequireAuthorization();

        return endpoints;
    }
}
