using SmartSchool.SharedKernel.Constants;
using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Activities.Features.Award;

public static class GetAwardById
{
    public sealed record Response(
        Guid TenantId,
        Guid Id,
        Guid StudentId,
        string StudentNumber,
        string StudentName,
        string AwardTypeCode,
        string Title,
        string? Description,
        DateOnly AwardDate,
        Guid? ApprovedBy,
        string? ApprovedByName,
        Guid? DocumentId);

    public sealed record Query(Guid Id, Guid? TenantId) : IRequest<Result<Response>>;

    public interface IGetAwardById
    {
        Task<Response?> ExecuteAsync(Guid tenantId, Guid id, CancellationToken cancellationToken);
    }

    internal sealed class GetAwardByIdQuery(IDbConnectionFactory connectionFactory) : IGetAwardById
    {
        public async Task<Response?> ExecuteAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    a.tenant_id AS "TenantId",
                    a.student_award_id AS "Id",
                    a.student_id AS "StudentId",
                    s.student_number AS "StudentNumber",
                    trim(concat_ws(' ', s.first_name, s.last_name)) AS "StudentName",
                    a.award_type_code AS "AwardTypeCode",
                    a.title AS "Title",
                    a.description AS "Description",
                    a.award_date AS "AwardDate",
                    a.approved_by AS "ApprovedBy",
                    NULLIF(trim(concat_ws(' ', e.first_name, e.last_name)), '') AS "ApprovedByName",
                    a.generated_document_id AS "DocumentId"
                FROM activity.student_award a
                JOIN student.student s ON s.student_id=a.student_id AND s.tenant_id=a.tenant_id
                LEFT JOIN hr.employee e ON e.employee_id=a.approved_by AND e.tenant_id=a.tenant_id
                WHERE a.tenant_id=@TenantId AND a.student_award_id=@Id AND a.is_active=TRUE;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<Response>(
                new CommandDefinition(sql, new { TenantId = tenantId, Id = id }, cancellationToken: cancellationToken));
        }
    }

    public sealed class Handler(IGetAwardById query, ITenantScope tenantScope)
        : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Query request, CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
            {
                return Result<Response>.Failure(Error.Validation("Tenant context is required."));
            }

            var result = await query.ExecuteAsync(tenantId.Value, request.Id, cancellationToken);
            return result is null
                ? Result<Response>.Failure(Error.NotFound("Award was not found."))
                : Result<Response>.Success(result);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "award"),
                async (Guid id, Guid? tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                    (await mediator.SendAsync<Query, Result<Response>>(new Query(id, tenantId), cancellationToken)).ToHttpResult())
            .WithName("GetAwardById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
