using SmartSchool.Application.Messaging;
using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Teacher.GetTeacherProfile;

public static class GetTeacherProfile
{
    public sealed record Request(Guid TenantId, Guid UserId) : IRequest<Response>;
    public sealed record Response(IReadOnlyList<dynamic> Items);

    public interface IGetTeacherProfileQuery
    {
        Task<Response> ExecuteAsync(Guid tenantId, Guid userId, CancellationToken cancellationToken);
    }

    internal sealed class GetTeacherProfileQuery(IDbConnectionFactory connectionFactory) : IGetTeacherProfileQuery
    {
        public async Task<Response> ExecuteAsync(Guid tenantId, Guid userId, CancellationToken cancellationToken)
        {
            const string sql = """
            SELECT employee_id AS "EmployeeId", tenant_id AS "TenantId", user_id AS "UserId", employee_number AS "EmployeeNumber", first_name AS "FirstName", last_name AS "LastName", email AS "Email", phone AS "Phone", status AS "Status" FROM hr.employee WHERE tenant_id = @TenantId AND user_id = @UserId LIMIT 1;
            """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var rows = await connection.QueryAsync(new CommandDefinition(sql, new { TenantId = tenantId, UserId = userId }, cancellationToken: cancellationToken));
            return new Response(rows.ToList());
        }
    }

    public sealed class Handler(IGetTeacherProfileQuery query) : IRequestHandler<Request, Response>
    {
        public Task<Response> HandleAsync(Request request, CancellationToken cancellationToken) => query.ExecuteAsync(request.TenantId, request.UserId, cancellationToken);
    }

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/me", HandleAsync).RequireAuthorization(SmartSchoolPolicies.TeacherWorkspace);
    }

    private static async Task<IResult> HandleAsync(ITenantScope tenantScope, IMediator mediator, CancellationToken cancellationToken)
    {
            var tenantId = tenantScope.TenantId ?? Guid.Empty;
            var response = await mediator.SendAsync<Request, Response>(new Request(tenantId, tenantScope.UserId), cancellationToken);
            return Results.Ok(response.Items);
    }
}
