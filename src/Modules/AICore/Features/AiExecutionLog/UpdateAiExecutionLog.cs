using SmartSchool.Modules.AICore.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AICore.Features.AiExecutionLog;

public static class UpdateAiExecutionLog
{
    /// <summary>
    /// Represents the response returned by this AiExecutionLogEntity feature.
    /// </summary>
    /// <param name="TenantId">The owning tenant identifier.</param>
    /// <param name="Id">The entity identifier.</param>
    /// <param name="Code">The business code.</param>
    /// <param name="Name">The display name.</param>
    public sealed record Response(
    Guid TenantId,
    Guid Id,
    string Code,
    string Name,
    string? MetadataJson);

    public sealed record Request(
        Guid TenantId,
        Guid Id,
        string Name) : IRequest<Result<Response>>;

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        }
    }

    public interface IUpdateAiExecutionLog
    {
        Task UpdateAsync(
                AiExecutionLogEntity entity,
                CancellationToken cancellationToken);
Task<AiExecutionLogEntity?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken);

    }

    internal sealed class UpdateAiExecutionLogPersistence(IAICoreDbContext dbContext) : IUpdateAiExecutionLog
    {
        public async Task UpdateAsync(
                AiExecutionLogEntity entity,
                CancellationToken cancellationToken)
            {
                dbContext.AiExecutionLogs
                    .Update(entity);

                await dbContext.SaveChangesAsync(cancellationToken);
            }

        public async Task<AiExecutionLogEntity?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken)
            {
                return await dbContext.AiExecutionLogs
                    .FirstOrDefaultAsync(
                        x => x.TenantId == tenantId
                            && x.AiExecutionLogId == id,
                        cancellationToken);
            }
    }

    public sealed class Handler(IUpdateAiExecutionLog dataAccess)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var entity = await dataAccess.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<Response>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(AiExecutionLogEntity))));
            }


            entity.UpdateDetails(
                entity.Code,
                request.Name);
            await dataAccess.UpdateAsync(entity, cancellationToken);
            return Result<Response>.Success(MapResponse(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "ai-execution-log"),
                async (Guid id, Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var command = request with { Id = id };
                    var result = await mediator.SendAsync<Request, Result<Response>>(
                        command, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("UpdateAiExecutionLog")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }

    private static Response MapResponse(AiExecutionLogEntity entity)
    {
        return new Response(
            entity.TenantId,
            entity.AiExecutionLogId,
            entity.Code,
            entity.Name,
            entity.MetadataJson);
    }
}
