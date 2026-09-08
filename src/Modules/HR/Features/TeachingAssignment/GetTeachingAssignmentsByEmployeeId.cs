using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Features.TeachingAssignment;

public static class GetTeachingAssignmentsByEmployeeId
{
    public sealed record Response(
        Guid TeacherTeachingAssignmentId,
        string Code,
        string Name,
        Guid EmployeeId,
        Guid CampusId,
        Guid ClassSectionId,
        Guid SubjectId,
        int? PeriodsPerWeek,
        bool IsClassTeacher);

    public sealed record Query(
        Guid TenantId,
        Guid EmployeeId)
        : IRequest<Result<IReadOnlyCollection<Response>>>;

    public interface IGetTeachingAssignmentsByEmployeeIdQuery
    {
        Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid employeeId,
            CancellationToken cancellationToken);
    }

    internal sealed class GetTeachingAssignmentsByEmployeeIdQuery(
        IDbConnectionFactory connectionFactory)
        : IGetTeachingAssignmentsByEmployeeIdQuery
    {
        public async Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid employeeId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    teacher_teaching_assignment_id AS "TeacherTeachingAssignmentId",
                    code AS "Code",
                    name AS "Name",
                    employee_id AS "EmployeeId",
                    campus_id AS "CampusId",
                    class_section_id AS "ClassSectionId",
                    subject_id AS "SubjectId",
                    periods_per_week AS "PeriodsPerWeek",
                    is_class_teacher AS "IsClassTeacher"
                FROM hr.teacher_teaching_assignment
                WHERE tenant_id = @TenantId
                  AND employee_id = @EmployeeId
                  AND is_active = TRUE
                ORDER BY name;
                """;

            await using var connection =
                await connectionFactory.OpenConnectionAsync(cancellationToken);

            var assignments = await connection.QueryAsync<Response>(
                new CommandDefinition(
                    sql,
                    new
                    {
                        TenantId = tenantId,
                        EmployeeId = employeeId
                    },
                    cancellationToken: cancellationToken));

            return assignments.AsList();
        }
    }

    public sealed class Handler(
        IGetTeachingAssignmentsByEmployeeIdQuery query)
        : IRequestHandler<Query, Result<IReadOnlyCollection<Response>>>
    {
        public async Task<Result<IReadOnlyCollection<Response>>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var assignments = await query.GetAsync(
                request.TenantId,
                request.EmployeeId,
                cancellationToken);

            return Result<IReadOnlyCollection<Response>>.Success(assignments);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/hr/teaching-assignment/by-employee/{employeeId:guid}",
                async (
                    Guid employeeId,
                    Guid tenantId,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var query = new Query(tenantId, employeeId);
                    var result = await mediator.SendAsync<
                        Query,
                        Result<IReadOnlyCollection<Response>>>(
                        query,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("GetTeachingAssignmentsByEmployeeId")
            .WithTags("HR")
            .RequireAuthorization();

        return endpoints;
    }
}
