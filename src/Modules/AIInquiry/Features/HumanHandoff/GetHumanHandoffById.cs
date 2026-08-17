using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIInquiry.Contracts;
using SmartSchool.Modules.AIInquiry.Models;
using SmartSchool.Modules.AIInquiry.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIInquiry.Features.HumanHandoff;

public static class GetHumanHandoffById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<HumanHandoffResponse>>;

    public sealed class Handler(IHumanHandoffQuery entityQuery)
        : IRequestHandler<Query, Result<HumanHandoffResponse>>
    {
        public async Task<Result<HumanHandoffResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<HumanHandoffResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(HumanHandoff))));
            }
            return Result<HumanHandoffResponse>.Success(HumanHandoffResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "human-handoff"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<HumanHandoffResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetHumanHandoffById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
