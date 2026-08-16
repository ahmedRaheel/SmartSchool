using FluentValidation;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIInquiry.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIInquiry.Features.InquiryConversation;

public static class CreateInquiryConversation
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
        IRepository<InquiryConversation> repository,
        IValidator<Request> validator)
    {
        public async Task<Result<InquiryConversation>> HandleAsync(
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

                return Result<InquiryConversation>.Failure(
                    Error.Validation(message));
            }

            var codeExists = await repository.ExistsByCodeAsync(
                request.TenantId,
                request.Code,
                excludingId: null,
                cancellationToken);

            if (codeExists)
            {
                return Result<InquiryConversation>.Failure(
                    Error.Conflict(
                        $"A InquiryConversation with code '{request.Code}' already exists."));
            }

            var entity = new InquiryConversation
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

            return Result<InquiryConversation>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "/api/aiinquiry/inquiry-conversation",
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
            .WithName("CreateInquiryConversation")
            .WithTags("AIInquiry")
            .RequireAuthorization();

        return endpoints;
    }
}
