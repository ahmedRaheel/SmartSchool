using SmartSchool.Application.Persistence;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Examinations.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Examinations.Features.ExamSubject;

public static class GetExamSubjectById
{
    /// <summary>
    /// Represents the response returned by this ExamSubjectEntity feature.
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
    Guid CourseOfferingId,
    string? CourseOfferingCode,
    string? CourseOfferingName,
    Guid ExamId,
    string? ExamCode,
    string? ExamName,
    Guid? RoomId,
    string? RoomCode,
    string? RoomName);

    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<Response>>;

    public interface IGetExamSubjectByIdQuery
    {
        Task<Response?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken);

    }

    internal sealed class GetExamSubjectByIdQuery(
        IDbConnectionFactory connectionFactory) : IGetExamSubjectByIdQuery
    {
        public async Task<Response?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken)
            {
                const string sql = """
                    SELECT
                        tenant_id AS "TenantId",
                        entity.exam_subject_id AS "Id",
                        entity.code AS "Code",
                        entity.name AS "Name",
                        entity.metadata_json AS "MetadataJson",
                        p1.course_offering_id AS "CourseOfferingId",
                        p1.code AS "CourseOfferingCode",
                        p1.name AS "CourseOfferingName",
                        p2.exam_id AS "ExamId",
                        p2.code AS "ExamCode",
                        p2.name AS "ExamName",
                        p3.room_id AS "RoomId",
                        p3.code AS "RoomCode",
                        p3.name AS "RoomName"
                    FROM exam.exam_subject AS entity
                    LEFT JOIN academic.course_offering AS p1
                        ON p1.course_offering_id = entity.course_offering_id
                    LEFT JOIN exam.exam AS p2
                        ON p2.exam_id = entity.exam_id
                    LEFT JOIN org.room AS p3
                        ON p3.room_id = entity.room_id
                    WHERE tenant_id = @TenantId
                      AND entity.exam_subject_id = @Id
                      AND is_active = TRUE;
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

    public sealed class Handler(IGetExamSubjectByIdQuery query)
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
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(ExamSubjectEntity))));
            }
            return Result<Response>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "exam-subject"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetExamSubjectById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
