using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Inventory.Contracts;
using SmartSchool.Modules.Inventory.Models;
using SmartSchool.Modules.Inventory.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Inventory.Features.StockTransaction;

public static class UpdateStockTransaction
{
    public sealed record Request(
        Guid TenantId,
        Guid Id,
        string Code,
        string Name,
        bool IsActive) : IRequest<Result<StockTransactionResponse>>;

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Code).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        }
    }

    public sealed class Handler(
        IStockTransactionQuery entityQuery,
        IStockTransactionCommand entityCommand,
        IValidator<Request> validator)
        : IRequestHandler<Request, Result<StockTransactionResponse>>
    {
        public async Task<Result<StockTransactionResponse>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validation.Errors.Select(error => error.ErrorMessage));
                return Result<StockTransactionResponse>.Failure(Error.Validation(message));
            }

            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<StockTransactionResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(StockTransaction))));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, request.Id, cancellationToken);
            if (exists)
            {
                return Result<StockTransactionResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(StockTransaction), request.Code)));
            }

            entity.Code = request.Code.Trim();
            entity.Name = request.Name.Trim();
            entity.IsActive = request.IsActive;
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            await entityCommand.UpdateAsync(entity, cancellationToken);
            return Result<StockTransactionResponse>.Success(StockTransactionResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "stock-transaction"),
                async (Guid id, Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var command = request with { Id = id };
                    var result = await mediator.SendAsync<Request, Result<StockTransactionResponse>>(
                        command, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("UpdateStockTransaction")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
