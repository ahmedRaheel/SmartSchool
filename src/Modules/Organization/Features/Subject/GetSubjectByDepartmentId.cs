using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Features.Subject;

public static class GetSubjectByDepartmentId
{
    public sealed record Response(Guid TenantId, Guid Id, string Code, string Name);
    public sealed record Query(Guid TenantId, Guid DepartmentId) : IRequest<Result<IReadOnlyCollection<Response>>>;
    public interface IGetSubjectByDepartmentIdQuery
    {
        Task<IReadOnlyCollection<Response>> GetAsync(Guid tenantId, Guid departmentId, CancellationToken cancellationToken);
    }
    internal sealed class GetSubjectByDepartmentIdQuery(IDbConnectionFactory connectionFactory) : IGetSubjectByDepartmentIdQuery
    {
        public async Task<IReadOnlyCollection<Response>> GetAsync(Guid tenantId, Guid departmentId, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT tenant_id AS "TenantId", subject_id AS "Id", code AS "Code", name AS "Name"
                FROM academic.subject
                WHERE tenant_id = @TenantId AND department_id = @DepartmentId AND is_active = TRUE
                ORDER BY name;
                """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return (await connection.QueryAsync<Response>(new CommandDefinition(sql, new { TenantId = tenantId, DepartmentId = departmentId }, cancellationToken: cancellationToken))).AsList();
        }
    }
    public sealed class Handler(IGetSubjectByDepartmentIdQuery query) : IRequestHandler<Query, Result<IReadOnlyCollection<Response>>>
    {
        public async Task<Result<IReadOnlyCollection<Response>>> HandleAsync(Query request, CancellationToken cancellationToken) =>
            Result<IReadOnlyCollection<Response>>.Success(await query.GetAsync(request.TenantId, request.DepartmentId, cancellationToken));
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/academics/subject/by-department/{departmentId:guid}", async (Guid departmentId, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
            (await mediator.SendAsync<Query, Result<IReadOnlyCollection<Response>>>(new Query(tenantId, departmentId), cancellationToken)).ToHttpResult())
            .WithName("GetSubjectByDepartmentId").WithTags("Organization").RequireAuthorization();
        return endpoints;
    }
}
