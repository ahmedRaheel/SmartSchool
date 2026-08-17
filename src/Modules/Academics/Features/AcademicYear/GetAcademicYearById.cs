using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Academics.Contracts;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.Modules.Academics.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.AcademicYear;

public static class GetAcademicYearById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<AcademicYearResponse>>;

    public sealed class Handler(IAcademicYearQuery entityQuery)
        : IRequestHandler<Query, Result<AcademicYearResponse>>
    {
        public async Task<Result<AcademicYearResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<AcademicYearResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(AcademicYear))));
            }
            return Result<AcademicYearResponse>.Success(AcademicYearResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "academic-year"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<AcademicYearResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetAcademicYearById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
