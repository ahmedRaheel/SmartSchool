using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Activities.Contracts;
using SmartSchool.Modules.Activities.Models;
using SmartSchool.Modules.Activities.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Activities.Features.Award;

public static class CreateAward
{
    public sealed record Request(
        Guid TenantId,
        string Code,
        string Name) : IRequest<Result<AwardResponse>>;

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
        IAwardQuery entityQuery,
        IAwardCommand entityCommand,
        IValidator<Request> validator)
        : IRequestHandler<Request, Result<AwardResponse>>
    {
        public async Task<Result<AwardResponse>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validation.Errors.Select(error => error.ErrorMessage));
                return Result<AwardResponse>.Failure(Error.Validation(message));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, null, cancellationToken);
            if (exists)
            {
                return Result<AwardResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(Award), request.Code)));
            }

            var entity = new Award
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await entityCommand.AddAsync(entity, cancellationToken);
            return Result<AwardResponse>.Success(AwardResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "award"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<AwardResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateAward")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
