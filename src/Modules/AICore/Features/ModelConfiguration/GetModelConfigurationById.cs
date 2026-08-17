using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AICore.Contracts;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.Modules.AICore.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AICore.Features.ModelConfiguration;

public static class GetModelConfigurationById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<ModelConfigurationResponse>>;

    public sealed class Handler(IModelConfigurationQuery entityQuery)
        : IRequestHandler<Query, Result<ModelConfigurationResponse>>
    {
        public async Task<Result<ModelConfigurationResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<ModelConfigurationResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(ModelConfiguration))));
            }
            return Result<ModelConfigurationResponse>.Success(ModelConfigurationResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "model-configuration"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<ModelConfigurationResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetModelConfigurationById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
