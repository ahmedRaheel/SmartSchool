using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIInquiry.Contracts;
using SmartSchool.Modules.AIInquiry.Models;
using SmartSchool.Modules.AIInquiry.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIInquiry.Features.InquiryMessage;

public static class GetInquiryMessageById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<InquiryMessageResponse>>;

    public sealed class Handler(IInquiryMessageQuery entityQuery)
        : IRequestHandler<Query, Result<InquiryMessageResponse>>
    {
        public async Task<Result<InquiryMessageResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<InquiryMessageResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(InquiryMessage))));
            }
            return Result<InquiryMessageResponse>.Success(InquiryMessageResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "inquiry-message"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<InquiryMessageResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetInquiryMessageById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
