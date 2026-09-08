using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Features.Student;

public static class GetStudentsByCurrentCampus
{
    public sealed record Response(
        Guid StudentId,
        string? StudentNumber,
        string FirstName,
        string? LastName,
        string Status);

    public sealed record Query(
        Guid TenantId,
        Guid CampusId)
        : IRequest<Result<IReadOnlyCollection<Response>>>;

    public interface IGetStudentsByCurrentCampusQuery
    {
        Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid campusId,
            CancellationToken cancellationToken);
    }

    internal sealed class GetStudentsByCurrentCampusQuery(
        IDbConnectionFactory connectionFactory)
        : IGetStudentsByCurrentCampusQuery
    {
        public async Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid campusId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    student_id AS "StudentId",
                    student_number AS "StudentNumber",
                    first_name AS "FirstName",
                    last_name AS "LastName",
                    status AS "Status"
                FROM student.student
                WHERE tenant_id = @TenantId
                  AND branch_id = @CampusId
                  AND is_active = TRUE
                ORDER BY first_name, last_name;
                """;

            await using var connection =
                await connectionFactory.OpenConnectionAsync(cancellationToken);

            var students = await connection.QueryAsync<Response>(
                new CommandDefinition(
                    sql,
                    new
                    {
                        TenantId = tenantId,
                        CampusId = campusId
                    },
                    cancellationToken: cancellationToken));

            return students.AsList();
        }
    }

    public sealed class Handler(
        IGetStudentsByCurrentCampusQuery query)
        : IRequestHandler<Query, Result<IReadOnlyCollection<Response>>>
    {
        public async Task<Result<IReadOnlyCollection<Response>>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var students = await query.GetAsync(
                request.TenantId,
                request.CampusId,
                cancellationToken);

            return Result<IReadOnlyCollection<Response>>.Success(students);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/students/student/by-current-campus",
                async (
                    ICurrentUser currentUser,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    if (currentUser.TenantId is not Guid tenantId ||
                        currentUser.BranchId is not Guid campusId)
                    {
                        return Results.Forbid();
                    }

                    var query = new Query(tenantId, campusId);
                    var result = await mediator.SendAsync<
                        Query,
                        Result<IReadOnlyCollection<Response>>>(
                        query,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("GetStudentsByCurrentCampus")
            .WithTags("Students")
            .RequireAuthorization();

        return endpoints;
    }
}
