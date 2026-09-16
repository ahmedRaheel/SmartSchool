using SmartSchool.Application.Identity;
using SmartSchool.SharedKernel.Constants;
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
        IDbConnectionFactory connectionFactory, ICurrentUser user)
        : IGetTeachingAssignmentsByEmployeeIdQuery
    {
        public async Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid employeeId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT a.teacher_course_assignment_id AS "TeacherTeachingAssignmentId", a.code AS "Code", a.name AS "Name",
                    a.employee_id AS "EmployeeId", cs.campus_id AS "CampusId", a.class_section_id AS "ClassSectionId",
                    co.subject_id AS "SubjectId", a.periods_per_week AS "PeriodsPerWeek",
                    coalesce(cs.class_teacher_employee_id=a.employee_id,false) AS "IsClassTeacher"
                FROM academic.teacher_course_assignment a
                JOIN academic.course_offering co ON co.course_offering_id=a.course_offering_id AND co.tenant_id=a.tenant_id
                JOIN academic.class_section cs ON cs.class_section_id=a.class_section_id AND cs.tenant_id=a.tenant_id
                WHERE a.tenant_id=@TenantId AND a.employee_id=@EmployeeId AND a.is_active
                    AND (@ScopeBranchId IS NULL OR cs.campus_id=@ScopeBranchId)
                    AND (@TeacherId IS NULL OR a.employee_id=@TeacherId)
                ORDER BY a.name;
                """;

            await using var connection =
                await connectionFactory.OpenConnectionAsync(cancellationToken);

            var assignments = await connection.QueryAsync<Response>(
                new CommandDefinition(
                    sql,
                    new
                    {
                        TenantId = tenantId,
                        ScopeBranchId = user.BranchId,
                        TeacherId = user.IsInRole(SmartSchoolRoles.Teacher) ? user.EmployeeId ?? user.TeacherId ?? Guid.Empty : (Guid?)null,
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
            .RequireAuthorization(SmartSchoolPolicies.AcademicManagement);

        return endpoints;
    }
}
