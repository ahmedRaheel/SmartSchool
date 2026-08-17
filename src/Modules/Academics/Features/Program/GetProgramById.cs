using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Academics.Contracts;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.Modules.Academics.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.Program;

public static class GetProgramById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<ProgramResponse>>;

    public sealed class Handler(IProgramQuery entityQuery)
        : IRequestHandler<Query, Result<ProgramResponse>>
    {
        public async Task<Result<ProgramResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<ProgramResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Program))));
            }
            return Result<ProgramResponse>.Success(ProgramResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "program"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<ProgramResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetProgramById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
