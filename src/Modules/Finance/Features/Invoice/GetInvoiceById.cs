using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Finance.Contracts;
using SmartSchool.Modules.Finance.Models;
using SmartSchool.Modules.Finance.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Finance.Features.Invoice;

public static class GetInvoiceById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<InvoiceResponse>>;

    public sealed class Handler(IInvoiceQuery entityQuery)
        : IRequestHandler<Query, Result<InvoiceResponse>>
    {
        public async Task<Result<InvoiceResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<InvoiceResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Invoice))));
            }
            return Result<InvoiceResponse>.Success(InvoiceResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "invoice"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<InvoiceResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetInvoiceById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
