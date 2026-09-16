using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Activities.Features.Activity;

public static class GetActivityById
{
    public sealed record Response(
        Guid TenantId,
        Guid Id,
        string Code,
        string Name,
        string Category,
        DateOnly ActivityDate,
        TimeOnly? StartTime,
        TimeOnly? EndTime,
        string? Venue,
        string? Description,
        int? MaxParticipants,
        string Status,
        Guid? CampusId,
        string? CampusName,
        Guid? CoordinatorEmployeeId,
        string? CoordinatorName,
        int ParticipantCount);

    public sealed record Query(Guid Id, Guid? TenantId) : IRequest<Result<Response>>;

    public interface IGetActivityById
    {
        Task<Response?> ExecuteAsync(Guid tenantId, Guid id, CancellationToken cancellationToken);
    }

    internal sealed class GetActivityByIdQuery(IDbConnectionFactory connectionFactory) : IGetActivityById
    {
        public async Task<Response?> ExecuteAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    a.tenant_id AS "TenantId",
                    a.activity_id AS "Id",
                    a.code AS "Code",
                    a.name AS "Name",
                    a.category AS "Category",
                    a.activity_date AS "ActivityDate",
                    a.start_time AS "StartTime",
                    a.end_time AS "EndTime",
                    a.venue AS "Venue",
                    a.description AS "Description",
                    a.max_participants AS "MaxParticipants",
                    a.status AS "Status",
                    a.campus_id AS "CampusId",
                    c.name AS "CampusName",
                    a.coordinator_employee_id AS "CoordinatorEmployeeId",
                    NULLIF(trim(concat_ws(' ', e.first_name, e.last_name)), '') AS "CoordinatorName",
                    COALESCE((SELECT COUNT(*)::int FROM activity.student_activity sa
                              WHERE sa.tenant_id=a.tenant_id AND sa.activity_id=a.activity_id AND sa.is_active=TRUE AND sa.left_at IS NULL),0) AS "ParticipantCount"
                FROM activity.activity a
                LEFT JOIN org.campus c ON c.campus_id=a.campus_id
                LEFT JOIN hr.employee e ON e.employee_id=a.coordinator_employee_id
                WHERE a.tenant_id=@TenantId AND a.activity_id=@Id AND a.is_active=TRUE;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<Response>(new CommandDefinition(sql, new { TenantId = tenantId, Id = id }, cancellationToken: cancellationToken));
        }
    }

    public sealed class Handler(IGetActivityById query, ITenantScope tenantScope)
        : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Query request, CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
            {
                return Result<Response>.Failure(Error.Validation("Tenant context is required."));
            }

            var response = await query.ExecuteAsync(tenantId.Value, request.Id, cancellationToken);
            return response is null
                ? Result<Response>.Failure(Error.NotFound("Activity was not found."))
                : Result<Response>.Success(response);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "activity"),
                async (Guid id, Guid? tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                    (await mediator.SendAsync<Query, Result<Response>>(new Query(id, tenantId), cancellationToken)).ToHttpResult())
            .WithName("GetActivityById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
