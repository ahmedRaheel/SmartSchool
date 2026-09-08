using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Features.ClassSection;

public static class GetClassSectionByAcademicYearId
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
    Guid CampusId,
    string? CampusCode,
    string? CampusName,
    Guid? RoomId,
    string? RoomCode,
    string? RoomName,
    Guid SectionId,
    string? SectionCode,
    string? SectionName,
    Guid GradeLevelId,
    string? GradeLevelCode,
    string? GradeLevelName);

    public sealed record Query(Guid TenantId, Guid ParentId)
        : IRequest<Result<IReadOnlyCollection<Response>>>;

    public interface IGetClassSectionByAcademicYearIdQuery
    {
        Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid parentId,
            CancellationToken cancellationToken);
    }

    internal sealed class GetClassSectionByAcademicYearIdQuery(IDbConnectionFactory connectionFactory)
        : IGetClassSectionByAcademicYearIdQuery
    {
        public async Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid parentId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                    SELECT
                        entity.tenant_id AS "TenantId",
                        entity.class_section_id AS "Id",
                        entity.code AS "Code",
                        entity.name AS "Name",
                        entity.metadata_json AS "MetadataJson",
                        p1.academic_year_id AS "AcademicYearId",
                        p1.code AS "AcademicYearCode",
                        p1.name AS "AcademicYearName",
                        p2.campus_id AS "CampusId",
                        p2.code AS "CampusCode",
                        p2.name AS "CampusName",
                        p3.room_id AS "RoomId",
                        p3.code AS "RoomCode",
                        p3.name AS "RoomName",
                        p4.section_id AS "SectionId",
                        p4.code AS "SectionCode",
                        p4.name AS "SectionName",
                        p5.grade_level_id AS "GradeLevelId",
                        p5.code AS "GradeLevelCode",
                        p5.name AS "GradeLevelName"
                    FROM academic.class_section AS entity
                    LEFT JOIN academic.academic_year AS p1
                        ON p1.academic_year_id = entity.academic_year_id
                    LEFT JOIN org.campus AS p2
                        ON p2.campus_id = entity.campus_id
                    LEFT JOIN org.room AS p3
                        ON p3.room_id = entity.room_id
                    LEFT JOIN academic.section AS p4
                        ON p4.section_id = entity.section_id
                    LEFT JOIN academic.grade_level AS p5
                        ON p5.grade_level_id = entity.grade_level_id
                    WHERE entity.tenant_id = @TenantId
                      AND entity.academic_year_id = @ParentId
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

    public sealed class Handler(IGetClassSectionByAcademicYearIdQuery query)
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
                "/api/organization/class-section/by-academic-year/{parentId:guid}",
                async (Guid parentId, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Query, Result<IReadOnlyCollection<Response>>>(
                        new Query(tenantId, parentId),
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetClassSectionByAcademicYearId")
            .WithTags("Organization")
            .RequireAuthorization();

        return endpoints;
    }
}
