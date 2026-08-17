using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AICore.Contracts;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.Modules.AICore.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AICore.Features.PromptTemplate;

public static class GetPromptTemplateById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<PromptTemplateResponse>>;

    public sealed class Handler(IPromptTemplateQuery entityQuery)
        : IRequestHandler<Query, Result<PromptTemplateResponse>>
    {
        public async Task<Result<PromptTemplateResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<PromptTemplateResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(PromptTemplate))));
            }
            return Result<PromptTemplateResponse>.Success(PromptTemplateResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "prompt-template"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<PromptTemplateResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetPromptTemplateById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
