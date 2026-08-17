using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Identity.Contracts;
using SmartSchool.Modules.Identity.Models;
using SmartSchool.Modules.Identity.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Identity.Features.UserProfile;

public static class UpdateUserProfile
{
    public sealed record Request(
        Guid TenantId,
        Guid Id,
        string Code,
        string Name,
        bool IsActive) : IRequest<Result<UserProfileResponse>>;

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

            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<UserProfileResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(UserProfile))));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, request.Id, cancellationToken);
            if (exists)
            {
                return Result<UserProfileResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(UserProfile), request.Code)));
            }

            entity.Code = request.Code.Trim();
            entity.Name = request.Name.Trim();
            entity.IsActive = request.IsActive;
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            await entityCommand.UpdateAsync(entity, cancellationToken);
            return Result<UserProfileResponse>.Success(UserProfileResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "user-profile"),
                async (Guid id, Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var command = request with { Id = id };
                    var result = await mediator.SendAsync<Request, Result<UserProfileResponse>>(
                        command, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("UpdateUserProfile")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
