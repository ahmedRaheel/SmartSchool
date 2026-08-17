using SmartSchool.Application.Messaging;
using SmartSchool.Modules.HR.Contracts;
using SmartSchool.Modules.HR.Models;
using SmartSchool.Modules.HR.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.EmploymentHistory;

public static class GetEmploymentHistoryById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<EmploymentHistoryResponse>>;

    public sealed class Handler(IEmploymentHistoryQuery entityQuery)
        : IRequestHandler<Query, Result<EmploymentHistoryResponse>>
    {
        public async Task<Result<EmploymentHistoryResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<EmploymentHistoryResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(EmploymentHistory))));
            }
            return Result<EmploymentHistoryResponse>.Success(EmploymentHistoryResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "employment-history"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<EmploymentHistoryResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetEmploymentHistoryById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
