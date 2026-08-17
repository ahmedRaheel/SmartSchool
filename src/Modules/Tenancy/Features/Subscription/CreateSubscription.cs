using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Tenancy.Contracts;
using SmartSchool.Modules.Tenancy.Models;
using SmartSchool.Modules.Tenancy.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Tenancy.Features.Subscription;

public static class CreateSubscription
{
    public sealed record Request(
        Guid TenantId,
        string Code,
        string Name) : IRequest<Result<SubscriptionResponse>>;

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
        ISubscriptionQuery entityQuery,
        ISubscriptionCommand entityCommand,
        IValidator<Request> validator)
        : IRequestHandler<Request, Result<SubscriptionResponse>>
    {
        public async Task<Result<SubscriptionResponse>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validation.Errors.Select(error => error.ErrorMessage));
                return Result<SubscriptionResponse>.Failure(Error.Validation(message));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, null, cancellationToken);
            if (exists)
            {
                return Result<SubscriptionResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(Subscription), request.Code)));
            }

            var entity = new Subscription
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await entityCommand.AddAsync(entity, cancellationToken);
            return Result<SubscriptionResponse>.Success(SubscriptionResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "subscription"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<SubscriptionResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateSubscription")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
