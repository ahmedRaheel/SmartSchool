using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Payroll.Contracts;
using SmartSchool.Modules.Payroll.Models;
using SmartSchool.Modules.Payroll.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Payroll.Features.Increment;

public static class GetIncrementById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<IncrementResponse>>;

    public sealed class Handler(IIncrementQuery entityQuery)
        : IRequestHandler<Query, Result<IncrementResponse>>
    {
        public async Task<Result<IncrementResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<IncrementResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Increment))));
            }
            return Result<IncrementResponse>.Success(IncrementResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "increment"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<IncrementResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetIncrementById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
