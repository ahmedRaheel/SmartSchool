using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Academics.Contracts;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.Modules.Academics.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.ClassSection;

public static class GetClassSectionById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<ClassSectionResponse>>;

    public sealed class Handler(IClassSectionQuery entityQuery)
        : IRequestHandler<Query, Result<ClassSectionResponse>>
    {
        public async Task<Result<ClassSectionResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<ClassSectionResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(ClassSection))));
            }
            return Result<ClassSectionResponse>.Success(ClassSectionResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "class-section"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<ClassSectionResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetClassSectionById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
