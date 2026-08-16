using FluentValidation;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Features.ConversationParticipant;

public static class CreateConversationParticipant
{
    public sealed record Request(
        Guid TenantId,
        string Code,
        string Name);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId)
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
        IRepository<ConversationParticipant> repository,
        IValidator<Request> validator)
    {
        public async Task<Result<ConversationParticipant>> HandleAsync(
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

                return Result<ConversationParticipant>.Failure(
                    Error.Validation(message));
            }

            var codeExists = await repository.ExistsByCodeAsync(
                request.TenantId,
                request.Code,
                excludingId: null,
                cancellationToken);

            if (codeExists)
            {
                return Result<ConversationParticipant>.Failure(
                    Error.Conflict(
                        $"A ConversationParticipant with code '{request.Code}' already exists."));
            }

            var entity = new ConversationParticipant
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await repository.AddAsync(
                entity,
                cancellationToken);

            await repository.SaveChangesAsync(
                cancellationToken);

            return Result<ConversationParticipant>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "/api/communication/conversation-participant",
                async (
                    Request request,
                    Handler handler,
                    CancellationToken cancellationToken) =>
                {
                    var result = await handler.HandleAsync(
                        request,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("CreateConversationParticipant")
            .WithTags("Communication")
            .RequireAuthorization();

        return endpoints;
    }
}
