using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Inventory.Contracts;
using SmartSchool.Modules.Inventory.Models;
using SmartSchool.Modules.Inventory.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Inventory.Features.PurchaseOrder;

public static class CreatePurchaseOrder
{
    public sealed record Request(
        Guid TenantId,
        string Code,
        string Name) : IRequest<Result<PurchaseOrderResponse>>;

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.Code).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        }
    }

    public sealed class Handler(
        IPurchaseOrderQuery entityQuery,
        IPurchaseOrderCommand entityCommand,
        IValidator<Request> validator)
        : IRequestHandler<Request, Result<PurchaseOrderResponse>>
    {
        public async Task<Result<PurchaseOrderResponse>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validation.Errors.Select(error => error.ErrorMessage));
                return Result<PurchaseOrderResponse>.Failure(Error.Validation(message));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, null, cancellationToken);
            if (exists)
            {
                return Result<PurchaseOrderResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(PurchaseOrder), request.Code)));
            }

            var entity = new PurchaseOrder
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await entityCommand.AddAsync(entity, cancellationToken);
            return Result<PurchaseOrderResponse>.Success(PurchaseOrderResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "purchase-order"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<PurchaseOrderResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreatePurchaseOrder")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
