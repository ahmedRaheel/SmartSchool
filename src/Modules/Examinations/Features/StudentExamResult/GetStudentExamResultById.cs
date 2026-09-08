using Dapper;
using SmartSchool.Application.Persistence;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Examinations.Models;

using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Examinations.Features.StudentExamResult;

public static class GetStudentExamResultById
{
    /// <summary>
    /// Represents the response returned by this StudentExamResultEntity feature.
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
    Guid ExamSubjectId,
    string? ExamSubjectCode,
    string? ExamSubjectName);

    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<Response>>;    public interface IGetStudentExamResultByIdQuery
    {
        Task<Result<Response>> ExecuteAsync(
            Query request,
            CancellationToken cancellationToken);
    }



    internal sealed class GetStudentExamResultByIdQuery(IDbConnectionFactory connectionFactory) : IGetStudentExamResultByIdQuery
    {
        public async Task<Result<Response>> ExecuteAsync(
            Query request,
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
                  AND entity.student_exam_result_id = @Id
                  AND is_active = TRUE;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var response = await connection.QuerySingleOrDefaultAsync<Response>(
                new CommandDefinition(sql, new { request.TenantId, request.Id }, cancellationToken: cancellationToken));

            if (response is null)
            {
                return Result<Response>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Response))));
            }

            return Result<Response>.Success(response);
        }
    }

    public sealed class Handler(IGetStudentExamResultByIdQuery query)
        : IRequestHandler<Query, Result<Response>>
    {
        public Task<Result<Response>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            return query.ExecuteAsync(request, cancellationToken);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "student-exam-result"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetStudentExamResultById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
