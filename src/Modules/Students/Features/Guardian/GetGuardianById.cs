using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Persistence;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Students.Models;

using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Features.Guardian;

public static class GetGuardianById
{
    public sealed record Response(
        Guid TenantId,
        Guid Id,
        Guid? UserId,
        string FullName,
        string? CnicNumber,
        string? Email,
        string? Phone);

    public sealed record Query(Guid TenantId, Guid Id) : IRequest<Result<Response>>;

    public sealed class Handler(IDbConnectionFactory connectionFactory) : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Query request, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT tenant_id AS "TenantId", guardian_id AS "Id", user_id AS "UserId", full_name AS "FullName", cnic_number AS "CnicNumber", email AS "Email", phone AS "Phone"
                FROM student.guardian
                WHERE tenant_id = @TenantId
                  AND guardian_id = @Id
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
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "guardian"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Query, Result<Response>>(new Query(tenantId, id), cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetGuardianById").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantStudent);
        return endpoints;
    }
}
