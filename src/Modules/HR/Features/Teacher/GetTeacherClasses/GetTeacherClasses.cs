using SmartSchool.Application.Messaging;
using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Teacher.GetTeacherClasses;

public static class GetTeacherClasses
{
    public sealed record Request(Guid TenantId, Guid EmployeeId) : IRequest<Response>;
    public sealed record Response(IReadOnlyList<dynamic> Items);

    public interface IGetTeacherClassesQuery
    {
        Task<Response> ExecuteAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken);
    }

    internal sealed class GetTeacherClassesQuery(IDbConnectionFactory connectionFactory) : IGetTeacherClassesQuery
    {
        public async Task<Response> ExecuteAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken)
        {
            const string sql = """
            SELECT a.teacher_course_assignment_id AS "AssignmentId", a.course_offering_id AS "CourseOfferingId", a.class_section_id AS "ClassSectionId", a.assignment_role AS "Role", a.periods_per_week AS "PeriodsPerWeek", a.effective_from AS "EffectiveFrom", a.effective_to AS "EffectiveTo" FROM academic.teacher_course_assignment a WHERE a.tenant_id = @TenantId AND a.employee_id = @EmployeeId AND (a.effective_to IS NULL OR a.effective_to >= CURRENT_DATE);
            """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var rows = await connection.QueryAsync(new CommandDefinition(sql, new { TenantId = tenantId, EmployeeId = employeeId }, cancellationToken: cancellationToken));
            return new Response(rows.ToList());
        }
    }

    public sealed class Handler(IGetTeacherClassesQuery query) : IRequestHandler<Request, Response>
    {
        public Task<Response> HandleAsync(Request request, CancellationToken cancellationToken) => query.ExecuteAsync(request.TenantId, request.EmployeeId, cancellationToken);
    }

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/{employeeId:guid}/classes", HandleAsync);
    }

    private static async Task<IResult> HandleAsync(Guid employeeId, Guid? tenantId, ITenantScope tenantScope, IMediator mediator, CancellationToken cancellationToken)
    {
            var resolvedTenantId = tenantScope.IsSuperAdmin ? tenantId : tenantScope.Resolve(tenantId);
            if (!resolvedTenantId.HasValue)
            {
                return Results.BadRequest(new { message = "Tenant is required." });
            }

            var response = await mediator.SendAsync<Request, Response>(new Request(resolvedTenantId.Value, employeeId), cancellationToken);
            return Results.Ok(response.Items);
    }
}
