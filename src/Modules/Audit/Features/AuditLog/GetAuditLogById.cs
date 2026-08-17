using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Audit.Contracts;
using SmartSchool.Modules.Audit.Models;
using SmartSchool.Modules.Audit.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Audit.Features.AuditLog;

public static class GetAuditLogById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<AuditLogResponse>>;

    public sealed class Handler(IAuditLogQuery entityQuery)
        : IRequestHandler<Query, Result<AuditLogResponse>>
    {
        public async Task<Result<AuditLogResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<AuditLogResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(AuditLog))));
            }
            return Result<AuditLogResponse>.Success(AuditLogResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "audit-log"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<AuditLogResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetAuditLogById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
