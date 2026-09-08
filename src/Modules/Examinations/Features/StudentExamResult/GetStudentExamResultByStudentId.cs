using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Examinations.Features.StudentExamResult;

public static class GetStudentExamResultByStudentId
{
    public sealed record Response(
    Guid TenantId,
    Guid Id,
    string Code,
    string Name,
    string? MetadataJson,
    Guid ExamSubjectId,
    string? ExamSubjectCode,
    string? ExamSubjectName);

    public sealed record Query(Guid TenantId, Guid ParentId)
        : IRequest<Result<IReadOnlyCollection<Response>>>;

    public interface IGetStudentExamResultByStudentIdQuery
    {
        Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid parentId,
            CancellationToken cancellationToken);
    }

    internal sealed class GetStudentExamResultByStudentIdQuery(IDbConnectionFactory connectionFactory)
        : IGetStudentExamResultByStudentIdQuery
    {
        public async Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid parentId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT tenant_id AS "TenantId", entity.student_exam_result_id AS "Id", entity.code AS "Code", entity.name AS "Name", entity.metadata_json::text AS "MetadataJson",
                        p1.exam_subject_id AS "ExamSubjectId",
                        p1.code AS "ExamSubjectCode",
                        p1.name AS "ExamSubjectName"
                FROM exam.student_exam_result AS entity
                    LEFT JOIN exam.exam_subject AS p1
                        ON p1.exam_subject_id = entity.exam_subject_id
                WHERE tenant_id = @TenantId
                  AND entity.student_id = @ParentId
                  AND is_active = TRUE;
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

    public sealed class Handler(IGetStudentExamResultByStudentIdQuery query)
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
                "/api/examinations/student-exam-result/by-student/{parentId:guid}",
                async (Guid parentId, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Query, Result<IReadOnlyCollection<Response>>>(
                        new Query(tenantId, parentId),
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetStudentExamResultByStudentId")
            .WithTags("Examinations")
            .RequireAuthorization();

        return endpoints;
    }
}
