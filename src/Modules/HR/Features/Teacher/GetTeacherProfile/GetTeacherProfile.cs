using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Teacher.GetTeacherProfile;

public static class GetTeacherProfile
{
    public sealed record Response(IReadOnlyList<dynamic> Items);

    public interface IGetTeacherProfileQuery
    {
        Task<Response> ExecuteAsync(Guid tenantId, CancellationToken cancellationToken);
    }

    internal sealed class GetTeacherProfileQuery(IDbConnectionFactory connectionFactory) : IGetTeacherProfileQuery
    {
        public async Task<Response> ExecuteAsync(Guid tenantId, CancellationToken cancellationToken)
        {
            const string sql = """
            SELECT employee_id AS "EmployeeId", tenant_id AS "TenantId", user_id AS "UserId", employee_number AS "EmployeeNumber", first_name AS "FirstName", last_name AS "LastName", email AS "Email", phone AS "Phone", status AS "Status" FROM hr.employee WHERE tenant_id = @TenantId AND user_id = @UserId LIMIT 1;
            """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var rows = await connection.QueryAsync(new CommandDefinition(sql, new { TenantId = tenantId }, cancellationToken: cancellationToken));
            return new Response(rows.ToList());
        }
    }

    public sealed class Handler(IGetTeacherProfileQuery query)
    {
        public Task<Response> HandleAsync(Guid tenantId, CancellationToken cancellationToken) => query.ExecuteAsync(tenantId, cancellationToken);
    }

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/me", HandleAsync).RequireAuthorization(SmartSchoolPolicies.TeacherWorkspace);
    }

    private static async Task<IResult> HandleAsync(ITenantScope tenantScope, Handler handler, CancellationToken cancellationToken)
    {
            var tenantId = tenantScope.TenantId ?? Guid.Empty;
            var response = await handler.HandleAsync(tenantId, cancellationToken);
            return Results.Ok(response.Items);
    }
}
