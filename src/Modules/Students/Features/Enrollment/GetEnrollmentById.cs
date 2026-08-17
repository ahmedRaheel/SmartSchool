using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Students.Contracts;
using SmartSchool.Modules.Students.Models;
using SmartSchool.Modules.Students.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Features.Enrollment;

public static class GetEnrollmentById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<EnrollmentResponse>>;

    public sealed class Handler(IEnrollmentQuery entityQuery)
        : IRequestHandler<Query, Result<EnrollmentResponse>>
    {
        public async Task<Result<EnrollmentResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<EnrollmentResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Enrollment))));
            }
            return Result<EnrollmentResponse>.Success(EnrollmentResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "enrollment"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<EnrollmentResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetEnrollmentById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
