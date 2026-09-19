using SmartSchool.Application.Messaging;
using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Teacher.GetTeacher;

public static class GetTeacher
{
    public sealed record Request(Guid TenantId, Guid EmployeeId) : IRequest<Response>;
    public sealed record Response(IReadOnlyList<dynamic> Items);

    public interface IGetTeacherQuery
    {
        Task<Response> ExecuteAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken);
    }

    internal sealed class GetTeacherQuery(IDbConnectionFactory connectionFactory) : IGetTeacherQuery
    {
        public async Task<Response> ExecuteAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken)
        {
            const string sql = """
            SELECT e.employee_id AS "EmployeeId", e.tenant_id AS "TenantId", e.user_id AS "UserId", e.employee_number AS "EmployeeNumber", e.first_name AS "FirstName", e.last_name AS "LastName", e.email AS "Email", e.phone AS "Phone", e.hire_date AS "HireDate", e.employment_type_code AS "EmploymentType", e.status AS "Status", (SELECT string_agg(ed.qualification, ', ' ORDER BY ed.qualification) FROM hr.employee_education ed WHERE ed.tenant_id = e.tenant_id AND ed.employee_id = e.employee_id) AS "Qualification" FROM hr.employee e WHERE e.tenant_id = @TenantId AND e.employee_id = @EmployeeId;
            """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var rows = await connection.QueryAsync(new CommandDefinition(sql, new { TenantId = tenantId, EmployeeId = employeeId }, cancellationToken: cancellationToken));
            return new Response(rows.ToList());
        }
    }

    public sealed class Handler(IGetTeacherQuery query) : IRequestHandler<Request, Response>
    {
        public Task<Response> HandleAsync(Request request, CancellationToken cancellationToken) => query.ExecuteAsync(request.TenantId, request.EmployeeId, cancellationToken);
    }

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/{employeeId:guid}", HandleAsync);
    }

    private static async Task<IResult> HandleAsync(Guid employeeId, Guid? tenantId, ITenantScope tenantScope, ICurrentUser currentUser, IMediator mediator, CancellationToken cancellationToken)
    {
            if (currentUser.IsInRole(SmartSchoolRoles.Teacher) && employeeId != (currentUser.EmployeeId ?? currentUser.TeacherId))
            {
                return Results.Forbid();
            }
            var resolvedTenantId = tenantScope.Resolve(tenantId);
            if (!resolvedTenantId.HasValue)
            {
                return Results.BadRequest(new { message = "Tenant is required." });
            }

            var response = await mediator.SendAsync<Request, Response>(new Request(resolvedTenantId.Value, employeeId), cancellationToken);
            return Results.Ok(response.Items);
    }
}
