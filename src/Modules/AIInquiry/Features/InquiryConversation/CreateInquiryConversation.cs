using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIInquiry.Contracts;
using SmartSchool.Modules.AIInquiry.Models;
using SmartSchool.Modules.AIInquiry.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIInquiry.Features.InquiryConversation;

public static class CreateInquiryConversation
{
    public sealed record Request(
        Guid TenantId,
        string Code,
        string Name) : IRequest<Result<InquiryConversationResponse>>;

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
        IInquiryConversationQuery entityQuery,
        IInquiryConversationCommand entityCommand,
        IValidator<Request> validator)
        : IRequestHandler<Request, Result<InquiryConversationResponse>>
    {
        public async Task<Result<InquiryConversationResponse>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validation.Errors.Select(error => error.ErrorMessage));
                return Result<InquiryConversationResponse>.Failure(Error.Validation(message));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, null, cancellationToken);
            if (exists)
            {
                return Result<InquiryConversationResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(InquiryConversation), request.Code)));
            }

            var entity = new InquiryConversation
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await entityCommand.AddAsync(entity, cancellationToken);
            return Result<InquiryConversationResponse>.Success(InquiryConversationResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "inquiry-conversation"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<InquiryConversationResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateInquiryConversation")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
