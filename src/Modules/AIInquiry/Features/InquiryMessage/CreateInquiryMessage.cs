using SmartSchool.Modules.AIInquiry;
using FluentValidation;
using SmartSchool.Modules.AIInquiry.Persistence;
using SmartSchool.Modules.AIInquiry.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIInquiry.Features.InquiryMessage;

public static class CreateInquiryMessage
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
        IInquiryMessageQuery query,
        IInquiryMessageCommand command,
        IValidator<Request> validator)
    {
        public async Task<Result<InquiryMessage>> HandleAsync(
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

                return Result<InquiryMessage>.Failure(
                    Error.Validation(message));
            }

            var codeExists = await query.ExistsByCodeAsync(
                request.TenantId,
                request.Code,
                excludingId: null,
                cancellationToken);

            if (codeExists)
            {
                return Result<InquiryMessage>.Failure(
                    Error.Conflict(ErrorMessages.DuplicateCode(nameof(InquiryMessage), request.Code)));
            }

            var entity = new InquiryMessage
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await command.AddAsync(
                entity,
                cancellationToken);

            return Result<InquiryMessage>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "inquiry-message"),
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
            .WithName("CreateInquiryMessage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
