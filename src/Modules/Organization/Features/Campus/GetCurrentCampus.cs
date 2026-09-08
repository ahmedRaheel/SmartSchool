using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Features.Campus;

public static class GetCurrentCampus
{
    public sealed record Response(
        Guid TenantId,
        Guid Id,
        Guid SchoolId,
        string Code,
        string Name);

    public sealed record Query(Guid TenantId, Guid CampusId)
        : IRequest<Result<Response>>;

    public interface IGetCurrentCampusQuery
    {
        Task<Response?> GetAsync(
            Guid tenantId,
            Guid campusId,
            CancellationToken cancellationToken);
    }

    internal sealed class GetCurrentCampusQuery(IDbConnectionFactory connectionFactory)
        : IGetCurrentCampusQuery
    {
        public async Task<Response?> GetAsync(
            Guid tenantId,
            Guid campusId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    tenant_id AS "TenantId",
                    campus_id AS "Id",
                    school_id AS "SchoolId",
                    code AS "Code",
                    name AS "Name"
                FROM org.campus
                WHERE tenant_id = @TenantId
                  AND campus_id = @CampusId
                  AND is_active = TRUE;
                """;

            await using var connection =
                await connectionFactory.OpenConnectionAsync(cancellationToken);

            return await connection.QuerySingleOrDefaultAsync<Response>(
                new CommandDefinition(
                    sql,
                    new { TenantId = tenantId, CampusId = campusId },
                    cancellationToken: cancellationToken));
        }
    }

    public sealed class Handler(IGetCurrentCampusQuery query)
        : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var campus = await query.GetAsync(
                request.TenantId,
                request.CampusId,
                cancellationToken);

            return campus is null
                ? Result<Response>.Failure(Error.NotFound("Campus.NotFound"))
                : Result<Response>.Success(campus);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/organization/campus/current",
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

                    var result = await mediator.SendAsync<Query, Result<Response>>(
                        new Query(tenantId, campusId),
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("GetCurrentCampus")
            .WithTags("Organization")
            .RequireAuthorization();

        return endpoints;
    }
}
