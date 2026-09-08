using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AITutor.Features.GeneratedQuiz;

public static class GetGeneratedQuizByStudentId
{
    public sealed record Response(
    Guid TenantId,
    Guid Id,
    string Code,
    string Name,
    string? MetadataJson,
    Guid SubjectId,
    string? SubjectCode,
    string? SubjectName,
    Guid? TutorConversationId,
    string? TutorConversationCode,
    string? TutorConversationName);

    public sealed record Query(Guid TenantId, Guid ParentId)
        : IRequest<Result<IReadOnlyCollection<Response>>>;

    public interface IGetGeneratedQuizByStudentIdQuery
    {
        Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid parentId,
            CancellationToken cancellationToken);
    }

    internal sealed class GetGeneratedQuizByStudentIdQuery(IDbConnectionFactory connectionFactory)
        : IGetGeneratedQuizByStudentIdQuery
    {
        public async Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid parentId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                    SELECT
                        entity.tenant_id AS "TenantId",
                        entity.generated_quiz_id AS "Id",
                        entity.code AS "Code",
                        entity.name AS "Name",
                        entity.metadata_json AS "MetadataJson",
                        p1.subject_id AS "SubjectId",
                        p1.code AS "SubjectCode",
                        p1.name AS "SubjectName",
                        p2.tutor_conversation_id AS "TutorConversationId",
                        p2.code AS "TutorConversationCode",
                        p2.name AS "TutorConversationName"
                    FROM ai_tutor.generated_quiz AS entity
                    LEFT JOIN academic.subject AS p1
                        ON p1.subject_id = entity.subject_id
                    LEFT JOIN ai_tutor.tutor_conversation AS p2
                        ON p2.tutor_conversation_id = entity.tutor_conversation_id
                    WHERE entity.tenant_id = @TenantId
                      AND entity.student_id = @ParentId
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

    public sealed class Handler(IGetGeneratedQuizByStudentIdQuery query)
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
                "/api/aitutor/generated-quiz/by-student/{parentId:guid}",
                async (Guid parentId, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Query, Result<IReadOnlyCollection<Response>>>(
                        new Query(tenantId, parentId),
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetGeneratedQuizByStudentId")
            .WithTags("AITutor")
            .RequireAuthorization();

        return endpoints;
    }
}
