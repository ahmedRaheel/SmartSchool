using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Identity.Contracts;
using SmartSchool.Modules.Identity.Models;
using SmartSchool.Modules.Identity.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Identity.Features.UserProfile;

public static class GetUserProfileById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<UserProfileResponse>>;

    public sealed class Handler(IUserProfileQuery entityQuery)
        : IRequestHandler<Query, Result<UserProfileResponse>>
    {
        public async Task<Result<UserProfileResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<UserProfileResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(UserProfile))));
            }
            return Result<UserProfileResponse>.Success(UserProfileResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "user-profile"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<UserProfileResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetUserProfileById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
