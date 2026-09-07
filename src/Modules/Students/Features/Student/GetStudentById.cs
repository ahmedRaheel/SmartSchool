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
        string Status);

    public sealed record Query(Guid TenantId, Guid Id) : IRequest<Result<Response>>;

    public sealed class Handler(IDbConnectionFactory connectionFactory) : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Query request, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT tenant_id AS "TenantId", student_id AS "Id", user_id AS "UserId", student_number AS "StudentNumber", first_name AS "FirstName", last_name AS "LastName", date_of_birth AS "DateOfBirth", gender AS "Gender", photo AS "Photo", photo_content_type AS "PhotoContentType", photo_file_name AS "PhotoFileName", admission_date AS "AdmissionDate", status AS "Status"
                FROM student.student
                WHERE tenant_id = @TenantId
                  AND student_id = @Id
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

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "student"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Query, Result<Response>>(new Query(tenantId, id), cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetStudentById").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantStudent);
        return endpoints;
    }
}
