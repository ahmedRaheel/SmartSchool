using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AICore.Contracts;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.Modules.AICore.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AICore.Features.AiExecutionLog;

public static class GetAiExecutionLogById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<AiExecutionLogResponse>>;

    public sealed class Handler(IAiExecutionLogQuery entityQuery)
        : IRequestHandler<Query, Result<AiExecutionLogResponse>>
    {
        public async Task<Result<AiExecutionLogResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<AiExecutionLogResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(AiExecutionLog))));
            }
            return Result<AiExecutionLogResponse>.Success(AiExecutionLogResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "ai-execution-log"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<AiExecutionLogResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetAiExecutionLogById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
