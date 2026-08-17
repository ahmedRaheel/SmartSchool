using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Admissions.Contracts;
using SmartSchool.Modules.Admissions.Models;
using SmartSchool.Modules.Admissions.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Admissions.Features.Inquiry;

public static class GetInquiryById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<InquiryResponse>>;

    public sealed class Handler(IInquiryQuery entityQuery)
        : IRequestHandler<Query, Result<InquiryResponse>>
    {
        public async Task<Result<InquiryResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<InquiryResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Inquiry))));
            }
            return Result<InquiryResponse>.Success(InquiryResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "inquiry"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<InquiryResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetInquiryById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
