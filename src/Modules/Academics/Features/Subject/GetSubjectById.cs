using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Academics.Contracts;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.Modules.Academics.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.Subject;

public static class GetSubjectById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<SubjectResponse>>;

    public sealed class Handler(ISubjectQuery entityQuery)
        : IRequestHandler<Query, Result<SubjectResponse>>
    {
        public async Task<Result<SubjectResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<SubjectResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Subject))));
            }
            return Result<SubjectResponse>.Success(SubjectResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "subject"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<SubjectResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetSubjectById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
