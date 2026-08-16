using FluentValidation;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Features.MessageReceipt;

public static class UpdateMessageReceipt
{
    public sealed record Request(
        Guid TenantId,
        Guid Id,
        string Code,
        string Name,
        bool IsActive);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId)
                .NotEmpty();

            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Code)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(250);
        }
    }

    public sealed class Handler(
        IRepository<MessageReceipt> repository,
        IValidator<Request> validator)
    {
        public async Task<Result<MessageReceipt>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validationResult =
                await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validationResult.Errors.Select(error => error.ErrorMessage));

                return Result<MessageReceipt>.Failure(
                    Error.Validation(message));
            }

            var entity = await repository.GetByIdAsync(
                request.TenantId,
                request.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<MessageReceipt>.Failure(
                    Error.NotFound("MessageReceipt was not found."));
            }

            var duplicateCode = await repository.ExistsByCodeAsync(
                request.TenantId,
                request.Code,
                request.Id,
                cancellationToken);

            if (duplicateCode)
            {
                return Result<MessageReceipt>.Failure(
                    Error.Conflict(
                        $"A MessageReceipt with code '{request.Code}' already exists."));
            }

            entity.Code = request.Code.Trim();
            entity.Name = request.Name.Trim();
            entity.IsActive = request.IsActive;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await repository.SaveChangesAsync(
                cancellationToken);

            return Result<MessageReceipt>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/api/communication/message-receipt/{id:guid}",
                async (
                    Guid id,
                    Request request,
                    Handler handler,
                    CancellationToken cancellationToken) =>
                {
                    var command = request with { Id = id };

                    var result = await handler.HandleAsync(
                        command,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("UpdateMessageReceipt")
            .WithTags("Communication")
            .RequireAuthorization();

        return endpoints;
    }
}
