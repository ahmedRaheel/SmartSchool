using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Identity.Contracts;
using SmartSchool.Modules.Identity.Models;
using SmartSchool.Modules.Identity.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Identity.Features.UserProfile;

public static class CreateUserProfile
{
    public sealed record Request(
        Guid TenantId,
        string Code,
        string Name) : IRequest<Result<UserProfileResponse>>;

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
        IUserProfileQuery entityQuery,
        IUserProfileCommand entityCommand,
        IValidator<Request> validator)
        : IRequestHandler<Request, Result<UserProfileResponse>>
    {
        public async Task<Result<UserProfileResponse>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validation.Errors.Select(error => error.ErrorMessage));
                return Result<UserProfileResponse>.Failure(Error.Validation(message));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, null, cancellationToken);
            if (exists)
            {
                return Result<UserProfileResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(UserProfile), request.Code)));
            }

            var entity = new UserProfile
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await entityCommand.AddAsync(entity, cancellationToken);
            return Result<UserProfileResponse>.Success(UserProfileResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "user-profile"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<UserProfileResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateUserProfile")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
