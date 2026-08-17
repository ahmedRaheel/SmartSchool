using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIInquiry.Contracts;
using SmartSchool.Modules.AIInquiry.Models;
using SmartSchool.Modules.AIInquiry.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIInquiry.Features.LeadCapture;

public static class GetLeadCaptureById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<LeadCaptureResponse>>;

    public sealed class Handler(ILeadCaptureQuery entityQuery)
        : IRequestHandler<Query, Result<LeadCaptureResponse>>
    {
        public async Task<Result<LeadCaptureResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<LeadCaptureResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(LeadCapture))));
            }
            return Result<LeadCaptureResponse>.Success(LeadCaptureResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "lead-capture"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<LeadCaptureResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetLeadCaptureById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
