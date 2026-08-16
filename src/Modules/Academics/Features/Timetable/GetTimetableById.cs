using SmartSchool.Modules.Academics;
using SmartSchool.Modules.Academics.Persistence;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.Timetable;

public static class GetTimetableById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        ITimetableQuery query)
    {
        public async Task<Result<Timetable>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await query.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<Timetable>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Timetable))));
            }

            return Result<Timetable>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/academics/timetable/{id:guid}",
                async (
                    Guid id,
                    Guid tenantId,
                    Handler handler,
                    CancellationToken cancellationToken) =>
                {
                    var query = new Query(tenantId, id);

                    var result = await handler.HandleAsync(
                        query,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("GetTimetableById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
