using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Academics.Contracts;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.Modules.Academics.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.TimetableEntry;

public static class GetTimetableEntryById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<TimetableEntryResponse>>;

    public sealed class Handler(ITimetableEntryQuery entityQuery)
        : IRequestHandler<Query, Result<TimetableEntryResponse>>
    {
        public async Task<Result<TimetableEntryResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<TimetableEntryResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(TimetableEntry))));
            }
            return Result<TimetableEntryResponse>.Success(TimetableEntryResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "timetable-entry"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<TimetableEntryResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetTimetableEntryById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
