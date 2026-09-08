using SmartSchool.Application.Messaging;
using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Teacher.GetTeacherAssignments;

public static class GetTeacherAssignments
{
    public sealed record Request(Guid TenantId, Guid EmployeeId) : IRequest<Response>;
    public sealed record Response(IReadOnlyList<dynamic> Items);

    public interface IGetTeacherAssignmentsQuery
    {
        Task<Response> ExecuteAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken);
    }

    internal sealed class GetTeacherAssignmentsQuery(IDbConnectionFactory connectionFactory) : IGetTeacherAssignmentsQuery
    {
        public async Task<Response> ExecuteAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken)
        {
            const string sql = """
            SELECT academic_assignment_id AS "AssignmentId", title AS "Title", assignment_type_code AS "Type", assigned_at AS "AssignedAt", due_at AS "DueAt", total_marks AS "TotalMarks", status AS "Status", class_section_id AS "ClassSectionId" FROM lms.academic_assignment WHERE tenant_id = @TenantId AND teacher_employee_id = @EmployeeId ORDER BY assigned_at DESC;
            """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var rows = await connection.QueryAsync(new CommandDefinition(sql, new { TenantId = tenantId, EmployeeId = employeeId }, cancellationToken: cancellationToken));
            return new Response(rows.ToList());
        }
    }

    public sealed class Handler(IGetTeacherAssignmentsQuery query) : IRequestHandler<Request, Response>
    {
        public Task<Response> HandleAsync(Request request, CancellationToken cancellationToken) => query.ExecuteAsync(request.TenantId, request.EmployeeId, cancellationToken);
    }

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/{employeeId:guid}/assignments", HandleAsync);
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
