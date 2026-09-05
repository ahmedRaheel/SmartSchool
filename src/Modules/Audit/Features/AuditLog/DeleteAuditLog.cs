using SmartSchool.Modules.Audit.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Audit.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Audit.Features.AuditLog;

public static class DeleteAuditLog
{
    public sealed record Command(
        Guid TenantId,
        long Id) : IRequest<Result<Response>>;

    public sealed record Response(
        Guid TenantId,
        long Id);

    public interface IDeleteAuditLog
    {
        Task DeleteAsync(
                AuditLogEntity entity,
                CancellationToken cancellationToken);

        Task<AuditLogEntity?> GetByIdAsync(
                Guid tenantId,
                long id,
                CancellationToken cancellationToken);

    }

    internal sealed class DeleteAuditLogPersistence(IAuditDbContext dbContext) : IDeleteAuditLog
    {
        public async Task DeleteAsync(
                AuditLogEntity entity,
                CancellationToken cancellationToken)
            {
                dbContext.AuditLogs
                    .Remove(entity);

                await dbContext.SaveChangesAsync(cancellationToken);
            }

        public async Task<AuditLogEntity?> GetByIdAsync(
                Guid tenantId,
                long id,
                CancellationToken cancellationToken)
            {
                return await dbContext.AuditLogs
                    .FirstOrDefaultAsync(
                        x => x.TenantId == tenantId
                            && x.AuditLogId == id,
                        cancellationToken);
            }
    }

    public sealed class Handler(IDeleteAuditLog dataAccess)
        : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Command request,
            CancellationToken cancellationToken)
        {
            var entity = await dataAccess.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<Response>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(AuditLogEntity))));
            }
            await dataAccess.DeleteAsync(entity, cancellationToken);
            return Result<Response>.Success(new Response(request.TenantId, request.Id));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "audit-log"),
                async (long id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Command(tenantId, id);
                    var result = await mediator.SendAsync<Command, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("DeleteAuditLog")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
