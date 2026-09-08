using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Persistence;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Students.Models;

using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Features.Student;

public static class GetStudentById
{
    public sealed record Response(
        Guid TenantId,
        Guid Id,
        Guid? UserId,
        string? StudentNumber,
        string FirstName,
        string? LastName,
        DateOnly? DateOfBirth,
        string? Gender,
        byte[]? Photo,
        string? PhotoContentType,
        string? PhotoFileName,
        DateOnly? AdmissionDate,
        string Status,
    Guid? SchoolId,
    string? SchoolCode,
    string? SchoolName);

    public sealed record Query(Guid TenantId, Guid Id) : IRequest<Result<Response>>;    public interface IGetStudentByIdQuery
    {
        Task<Result<Response>> ExecuteAsync(
            Query request,
            CancellationToken cancellationToken);
    }



    internal sealed class GetStudentByIdQuery(IDbConnectionFactory connectionFactory) : IGetStudentByIdQuery
    {
        public async Task<Result<Response>> ExecuteAsync(Query request, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT entity.tenant_id AS "TenantId", entity.student_id AS "Id", entity.user_id AS "UserId", entity.student_number AS "StudentNumber", entity.first_name AS "FirstName", entity.last_name AS "LastName", entity.date_of_birth AS "DateOfBirth", entity.gender AS "Gender", entity.photo AS "Photo", entity.photo_content_type AS "PhotoContentType", entity.photo_file_name AS "PhotoFileName", entity.admission_date AS "AdmissionDate", entity.status AS "Status",
                        p1.school_id AS "SchoolId",
                        p1.code AS "SchoolCode",
                        p1.name AS "SchoolName"
                FROM student.student AS entity
                    LEFT JOIN org.school AS p1
                        ON p1.school_id = entity.school_id
                WHERE entity.tenant_id = @TenantId
                  AND entity.student_id = @Id
                  AND entity.is_active = TRUE;
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

    public sealed class Handler(IGetStudentByIdQuery query)
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
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "student"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Query, Result<Response>>(new Query(tenantId, id), cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetStudentById").WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
