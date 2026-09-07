using SmartSchool.Application.Persistence;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AITutor.Features.TutorConversation;

public static class GetTutorConversationById
{
    /// <summary>
    /// Represents the response returned by this TutorConversationEntity feature.
    /// </summary>
    /// <param name="TenantId">The owning tenant identifier.</param>
    /// <param name="Id">The entity identifier.</param>
    /// <param name="Code">The business code.</param>
    /// <param name="Name">The display name.</param>
    public sealed record Response(
    Guid TenantId,
    Guid Id,
    string Code,
    string Name,
    string? MetadataJson,
    Guid? AcademicYearId,
    string? AcademicYearCode,
    string? AcademicYearName,
    Guid? CourseOfferingId,
    string? CourseOfferingCode,
    string? CourseOfferingName,
    Guid? SubjectId,
    string? SubjectCode,
    string? SubjectName);

    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<Response>>;

    public interface IGetTutorConversationByIdQuery
    {
        Task<Response?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken);

    }

    internal sealed class GetTutorConversationByIdQuery(
        IDbConnectionFactory connectionFactory) : IGetTutorConversationByIdQuery
    {
        public async Task<Response?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken)
            {
                const string sql = """
                    SELECT
                        entity.tenant_id AS "TenantId",
                        entity.tutor_conversation_id AS "Id",
                        entity.code AS "Code",
                        entity.name AS "Name",
                        entity.metadata_json AS "MetadataJson",
                        p1.academic_year_id AS "AcademicYearId",
                        p1.code AS "AcademicYearCode",
                        p1.name AS "AcademicYearName",
                        p2.course_offering_id AS "CourseOfferingId",
                        p2.code AS "CourseOfferingCode",
                        p2.name AS "CourseOfferingName",
                        p3.subject_id AS "SubjectId",
                        p3.code AS "SubjectCode",
                        p3.name AS "SubjectName"
                    FROM ai_tutor.tutor_conversation AS entity
                    LEFT JOIN academic.academic_year AS p1
                        ON p1.academic_year_id = entity.academic_year_id
                    LEFT JOIN academic.course_offering AS p2
                        ON p2.course_offering_id = entity.course_offering_id
                    LEFT JOIN academic.subject AS p3
                        ON p3.subject_id = entity.subject_id
                    WHERE entity.tenant_id = @TenantId
                      AND entity.tutor_conversation_id = @Id
                      AND entity.is_active = TRUE;
                    """;

                await using var connection =
                    await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

                return await connection.QuerySingleOrDefaultAsync<Response>(
                    new CommandDefinition(
                        sql,
                        new { TenantId = tenantId, Id = id },
                        cancellationToken: cancellationToken)).ConfigureAwait(false);
            }
    }

    public sealed class Handler(IGetTutorConversationByIdQuery query)
        : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await query.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<Response>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(TutorConversationEntity))));
            }
            return Result<Response>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "tutor-conversation"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetTutorConversationById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
